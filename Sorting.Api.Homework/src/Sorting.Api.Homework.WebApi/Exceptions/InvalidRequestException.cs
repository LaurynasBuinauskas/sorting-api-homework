namespace Sorting.Api.Homework.WebApi.Exceptions;

// Exception for an invalid array input string
public class InvalidRequestArrayException : Exception
{
    public InvalidRequestArrayException(string message) : base(message)
    {
    }
}
