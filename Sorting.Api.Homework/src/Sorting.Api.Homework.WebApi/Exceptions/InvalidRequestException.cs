namespace Sorting.Api.Homework.WebApi.Exceptions;

// Exception for an invalid input string
public class InvalidRequestArrayException : Exception
{
    public InvalidRequestArrayException(string message) : base(message)
    {
    }
}
