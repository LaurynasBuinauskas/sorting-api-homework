using Sorting.Api.Homework.WebApi.Models;
using System.IO.Abstractions;

namespace Sorting.Api.Homework.WebApi.InputOutput.Readers
{
    // Implementation of the file reader:
    public class FileReader : IFileReader
    {
        private readonly IFileSystem _fileSystem;

        public FileReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        // Method to read content from the latest file: 
        public async Task<LoadedFile> ReadLatestFile(string directoryName)
        {
            try
            {
                // Getting the directory
                var projectDirectory = AppContext.BaseDirectory;
                var directory = _fileSystem.Path.Combine(
                    projectDirectory,
                    directoryName);                

                // Getting all the files in that directory that end with .txt
                var files = _fileSystem.Directory.GetFiles(directory)
                    .Where(f => f.EndsWith(".txt"))
                    .OrderByDescending(f => 
                        _fileSystem.File.GetLastWriteTime(f));

                // Checking if any files were found
                if (!files.Any())
                {
                    throw new FileNotFoundException("No files found in the directory.");
                }

                // Getting the latest file and reading it's content
                var latestFile = files.First();
                var fileContent = await _fileSystem.File.ReadAllTextAsync(latestFile);

                return new LoadedFile
                {
                    SortedList = fileContent.Split(" ").Select(int.Parse),
                    FileName = Path.GetFileName(latestFile)
                };
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException($"An error occurred while trying to open a file: {e.Message}", e);
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException($"An error occurred while opening the directory: {e.Message}", e);
            }            
            catch (UnauthorizedAccessException e)
            {
                throw new UnauthorizedAccessException($"An error occurred while reading the file: {e.Message}", e);
            }            
            catch (IOException e)
            {
                throw new IOException($"An error occurred while reading the file: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured while reading sorted numbers: {e.Message}", e);
            }
        }        
    }
}