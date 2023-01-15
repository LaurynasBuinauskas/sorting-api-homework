using System.IO.Abstractions;

namespace Sorting.Api.Homework.WebApi.InputOutput.Writers;

// Implementation of the file writer:
public class FileWriter : IFileWriter
{
    private readonly IFileSystem _fileSystem;

    public FileWriter(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    // Method to write content in the specified file:        
    public async Task WriteToFile(string fileName, string directoryName, string content)
    {
        try
        {
            // Constructing the file path
            var projectDirectory = AppContext.BaseDirectory;
            var directory = _fileSystem.Path.Combine(projectDirectory, directoryName);

            if (!_fileSystem.Directory.Exists(directory))
            {
                _fileSystem.Directory.CreateDirectory(directory);
            }
            string filePath = _fileSystem.Path.Combine(directory, fileName);

            // Creating the file and writing to it
            await _fileSystem.File.WriteAllTextAsync(filePath, content);
        }
        catch (DirectoryNotFoundException e) 
        {
            throw new DirectoryNotFoundException($"An error occurred while creating a directory: {e.Message}", e);
        }
        catch (UnauthorizedAccessException e)
        {
            throw new UnauthorizedAccessException($"An error occurred while writing the submitted to a file: {e.Message}", e);
        }
        catch (IOException e)
        {
            throw new IOException($"An error occurred while writing the submitted to a file: {e.Message}", e);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occured while saving the submitted numbers: {e.Message}", e);
        }
    }
}
