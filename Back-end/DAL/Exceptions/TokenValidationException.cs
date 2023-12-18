namespace DAL.Exceptions
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message)
        : base(message)
        {

        }
    }
}
