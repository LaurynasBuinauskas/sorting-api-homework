namespace Sorting.Api.Homework.WebApi.Services;

// Interface for the sorting service
public interface ISortService
{
    Task<string> MergeSortAndSaveNumbers(string numbers);
}
