using Sorting.Api.Homework.WebApi.Models;

namespace Sorting.Api.Homework.WebApi.InputOutput.Readers
{
    // Interface for the file reader:
    public interface IFileReader
    {
        Task<LoadedFile> ReadLatestFile(string directoryName);
    }
}
