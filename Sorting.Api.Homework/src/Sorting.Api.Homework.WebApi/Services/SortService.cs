using Sorting.Api.Homework.WebApi.Algorithms;
using Sorting.Api.Homework.WebApi.Constants;
using Sorting.Api.Homework.WebApi.Exceptions;
using Sorting.Api.Homework.WebApi.Extensions;
using Sorting.Api.Homework.WebApi.InputOutput.Readers;
using Sorting.Api.Homework.WebApi.InputOutput.Writers;
using Sorting.Api.Homework.WebApi.Models;
using Sorting.Api.Homework.WebApi.Models.DTO;

namespace Sorting.Api.Homework.WebApi.Services;

// Implementation class for the sorting service
public class SortService : ISortService
{    
    private readonly ISortAlgorithm _sortAlgorithm;
    private readonly IFileWriter _fileWriter;
    private readonly IFileReader _fileReader;

    public SortService(ISortAlgorithm sortAlgorithm, 
        IFileWriter fileWriter, IFileReader fileReader)
    {
        _sortAlgorithm = sortAlgorithm;
        _fileWriter = fileWriter;
        _fileReader = fileReader;
    }

    // Method for applying the sorting algorithm for the input integer list
    // and saving it to a file
    public async Task<string> MergeSortAndSaveNumbers(string numbers)
    {
        // Check if the input string is valid
        if (!numbers.IsValidInputString())
        {
            throw new InvalidRequestArrayException("Invalid input string was submitted in the request");
        }

        try
        {
            var numberList = ParseNumbersString(numbers);

            // Applying the MergeSort algorithm to the input list
            _sortAlgorithm.Sort(numberList, AlgorithmNames.Merge);

            // Creating the file name
            var fileName = CreateFileName(AlgorithmNames.Merge, FileSaveDefaults.FileName);

            // Writing the content and creating the file
            await _fileWriter.WriteToFile(
                fileName, 
                FileSaveDefaults.DirecotoryName, 
                string.Join(" ", numberList));

            return $"Numbers sorted and saved to {fileName}";
        }        
        catch (Exception)
        {
            throw;
        }
    }

    // Method for applying the chsone sorting algorithm for the input integer list
    // and saving it to a file
    public async Task<string> ChooseSortAndSaveNumbers(SortRequestDTO request)
    {
        // Check if the request array string is valid
        if (!request.ArrayString.IsValidInputString())
        {
            throw new InvalidRequestArrayException("Invalid input string was submitted in the request");
        }
        // Check if the request algorithm is valid
        if (!request.SortAlgorithm.IsValidSortAlgorithm())
        {
            throw new InvalidAlgorithmException($"Invalid sort algorithm: " +
                $"{request.SortAlgorithm}. Available options are: " +
                $"{string.Join(", ", AlgorithmNames.Merge, AlgorithmNames.Inertion, AlgorithmNames.Bubble)}");
        }

        try
        {
            var numberList = ParseNumbersString(request.ArrayString);

            // Applying the chosen sorting algorithm to the input list
            _sortAlgorithm.Sort(numberList, request.SortAlgorithm);

            // Creating the file name
            var fileName = CreateFileName(request.SortAlgorithm, FileSaveDefaults.FileName);

            // Writing the content and creating the file
            await _fileWriter.WriteToFile(
                fileName,
                FileSaveDefaults.DirecotoryName,
                string.Join(" ", numberList));

            return $"Numbers sorted and saved to {fileName}";
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Method for laoding the latest saved file
    public async Task<LoadedFile> LoadLatestFile()
    {
        try
        {
            return await _fileReader.ReadLatestFile(FileSaveDefaults.DirecotoryName);
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Method for parsing a string containing an integer array
    private static List<int> ParseNumbersString(string numbers)
    {
        return numbers.Split(" ").Select(int.Parse).ToList();
    }

    // Method for creating the file name
    private static string CreateFileName(string algorithm, string fileName)
    {
        return $"{algorithm}_{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
    }

    // Method to get the list of algorithm names
    private static IEnumerable<string> GetAvailableAlgorithms()
    {
        return Enum.GetNames(typeof(AlgorithmNames));
    }
}
