using Sorting.Api.Homework.WebApi.Models;
using Sorting.Api.Homework.WebApi.Models.DTO;

namespace Sorting.Api.Homework.WebApi.Services;

// Interface for the sorting service
public interface ISortService
{
    Task<string> MergeSortAndSaveNumbers(string numbers);

    Task<string> ChooseSortAndSaveNumbers(SortRequestDTO request);

    Task<LoadedFile> LoadLatestFile();
}
