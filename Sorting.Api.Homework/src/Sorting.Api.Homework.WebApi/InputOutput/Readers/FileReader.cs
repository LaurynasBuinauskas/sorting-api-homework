using Sorting.Api.Homework.WebApi.Constants;
using Sorting.Api.Homework.WebApi.Models;
using System.IO;
using System.IO.Abstractions;

namespace Sorting.Api.Homework.WebApi.InputOutput.Readers
{
    public class FileReader : IFileReader
    {
        private readonly IFileSystem _fileSystem;

        public FileReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<LoadedFile> ReadLatestFile(string directoryName)
        {
            try
            {
                var projectDirectory = AppContext.BaseDirectory;
                var directory = _fileSystem.Path.Combine(
                    projectDirectory,
                    directoryName);                

                var files = _fileSystem.Directory.GetFiles(directory)
                    .Where(f => f.EndsWith(".txt"))
                    .OrderByDescending(f => 
                        _fileSystem.File.GetLastWriteTime(f));

                if (!files.Any())
                {
                    throw new FileNotFoundException("No files found in the directory.");
                }

                var latestFile = files.First();
                var fileContent = await _fileSystem.File.ReadAllTextAsync(latestFile);

                return new LoadedFile
                {
                    SortedList = fileContent.Split(" ").Select(int.Parse),
                    FileName = latestFile
                };
            }        
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException($"An error occurred while trying to open a file: {e.Message}", e);
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