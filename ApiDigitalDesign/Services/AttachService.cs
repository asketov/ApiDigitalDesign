using ApiDigitalDesign.Models.AttachModels;
using Common.Exceptions.Attach;
using Common.Exceptions.User;

namespace ApiDigitalDesign.Services
{
    public class AttachService
    {
        /// <summary>
        /// Add files to tempFolder.
        /// </summary>
        /// <param name="files"></param>
        /// <returns><see cref="MetadataModel"/></returns>
        /// <exception cref="FileAlreadyExistException">One of the files has already exist name.</exception>
        /// <exception cref="DirectoryNotExistException">TempDirectory don't exist.</exception>
        public async Task<List<MetadataModel>> UploadFiles(List<IFormFile> files)
        {
            var res = new List<MetadataModel>();
            foreach (var file in files)
            {
                var tempPath = Path.GetTempPath();
                var meta = new MetadataModel
                {
                    TempId = Guid.NewGuid(),
                    Name = file.FileName,
                    MimeType = file.ContentType,
                    Size = file.Length
                };
                res.Add(meta);
                var newPath = Path.Combine(tempPath, meta.TempId.ToString());
                var fileinfo = new FileInfo(newPath);
                if (fileinfo.Exists)
                {
                    throw new FileAlreadyExistException();
                }
                else
                {
                    if (fileinfo.Directory == null)
                    {
                        throw new DirectoryNotExistException();
                    }
                    else if (!fileinfo.Directory.Exists)
                    {
                        fileinfo.Directory?.Create();
                    }

                    using (var stream = System.IO.File.Create(newPath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Static function for create attach in folder attaches from tempFolder by tempId.
        /// </summary>
        /// <param name="TempId"></param>
        /// <returns>Path to attach in folder attaches.</returns>
        /// <exception cref="FileNotExistException">File in tempDirectory not found.</exception>
        public static string CopyTempFileToAttaches(Guid TempId)
        {
            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), TempId.ToString()));
            if (!tempFi.Exists) throw new FileNotExistException();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "attaches", TempId.ToString());
            var destFi = new FileInfo(path);
            if (destFi.Directory != null && !destFi.Directory.Exists) destFi.Directory.Create();
            System.IO.File.Copy(tempFi.FullName, path, true);
            return path;
        }
    }
}
