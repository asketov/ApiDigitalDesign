namespace Common.Exceptions.User
{
    /// <summary>
    /// Resourse is already exist
    /// </summary>
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string message) : base(message)
        {
            
        }
    }
}
