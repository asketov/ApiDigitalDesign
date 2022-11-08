namespace Common.Exceptions.User
{
    /// <summary>
    /// Resourse not found
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {

        }
    }
}
