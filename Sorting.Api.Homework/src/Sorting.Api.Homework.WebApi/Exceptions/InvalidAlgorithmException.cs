namespace Sorting.Api.Homework.WebApi.Exceptions
{
    // Exception for when an invalid algorithm was chosen
    public class InvalidAlgorithmException : Exception
    {
        public InvalidAlgorithmException(string message) : base(message)
        { 
        }
    }
}
