using Sorting.Api.Homework.WebApi.Models;

namespace Sorting.Api.Homework.WebApi.InputOutput.Readers
{
    public interface IFileReader
    {
        Task<LoadedFile> ReadLatestFile(string directoryName);
    }
}
