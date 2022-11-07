using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace ApiDigitalDesign.Services
{
    public class PostService
    {
        private readonly DataContext _db;
        public PostService(DataContext db)
        {
            _db = db;
        }

    }
}
