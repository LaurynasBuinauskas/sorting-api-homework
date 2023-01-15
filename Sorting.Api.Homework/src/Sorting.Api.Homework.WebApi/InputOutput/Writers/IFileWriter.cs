namespace Sorting.Api.Homework.WebApi.InputOutput.Writers;

// Interface for the file writer
public interface IFileWriter
{
    Task WriteToFile(string fileName, string direcotoryName, string content);
}
