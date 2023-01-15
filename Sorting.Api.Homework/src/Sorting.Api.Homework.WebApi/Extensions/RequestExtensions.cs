using Sorting.Api.Homework.WebApi.Constants;
using System.Linq;

namespace Sorting.Api.Homework.WebApi.Extensions;

// Extension class for string
public static class RequestExtensions
{
    // Method for validating the array input string
    public static bool IsValidInputString(this string numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers)) return false;
        var splitNumbers = numbers.Split(" ");
        return splitNumbers.All(x => int.TryParse(x, out _));
    }

    // Method for validating the submitted sorting algorithm
    public static bool IsValidSortAlgorithm(this string sortAlgorithm)
    {
        switch (sortAlgorithm)
        {
            case AlgorithmNames.Merge:
            case AlgorithmNames.Bubble:
            case AlgorithmNames.Inertion:
                return true;
            default:
                return false;
        }
    }
}
