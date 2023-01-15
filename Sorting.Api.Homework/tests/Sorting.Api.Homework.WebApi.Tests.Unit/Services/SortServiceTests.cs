using Sorting.Api.Homework.WebApi.Models.DTO;
using Sorting.Api.Homework.WebApi.Services;

namespace Sorting.Api.Homework.WebApi.Tests.Unit.Services;

internal class SortServiceTests
{
    protected SortService _sortService;
    protected Mock<ISortAlgorithm> _sortAlgorithmMock;
    protected Mock<IFileWriter> _fileWriterMock;
    protected Mock<IFileReader> _fileReaderMock;

    protected const string testString = "1 2 3 5 7 11 21 31 111";
    protected readonly int[] sortedArray = { 1, 2, 3, 5, 7, 11, 21, 31, 111 };

    protected const string fileName = "MergeSort_result";

    [SetUp]
    public void SetUp()
    {
        _sortAlgorithmMock = new Mock<ISortAlgorithm>();
        _fileWriterMock = new Mock<IFileWriter>();
        _fileReaderMock = new Mock<IFileReader>();

        _sortService = new SortService(_sortAlgorithmMock.Object, _fileWriterMock.Object, _fileReaderMock.Object);
    }

    [Test]
    public async Task Should_MergeSort_And_Save()
    {
        // Given
        _sortAlgorithmMock.Setup(x => x.Sort(sortedArray.ToList(), AlgorithmNames.Merge));
        _fileWriterMock.Setup(x => x.WriteToFile(It.IsAny<string>(), FileSaveDefaults.DirecotoryName, testString))
            .Returns(Task.CompletedTask);

        var expectedFileName = $"{AlgorithmNames.Merge}_{FileSaveDefaults.FileName}";

        // When
        var result = await _sortService.MergeSortAndSaveNumbers(testString);

        // Then
        Assert.That(result, Does.StartWith($"Numbers sorted and saved to {expectedFileName}"));
        _sortAlgorithmMock.Verify(x => x.Sort(sortedArray.ToList(), AlgorithmNames.Merge), Times.Once);
        _fileWriterMock.Verify(x => x.WriteToFile(It.IsAny<string>(), FileSaveDefaults.DirecotoryName, testString), Times.Once);
    }

    [Test]
    public async Task Should_Sort_By_Chosen_Algorithm_And_Save_Numbers()
    {
        // Given
        var request = new SortRequestDTO
        {
            ArrayString = testString,
            SortAlgorithm = AlgorithmNames.Bubble
        };

        _sortAlgorithmMock.Setup(x => x.Sort(sortedArray.ToList(), request.SortAlgorithm));

        _fileWriterMock.Setup(x => x.WriteToFile(fileName, FileSaveDefaults.DirecotoryName, testString))
            .Returns(Task.CompletedTask);

        var expectedFileName = $"{request.SortAlgorithm}_{FileSaveDefaults.FileName}";

        // When
        var result = await _sortService.ChooseSortAndSaveNumbers(request);

        // Then
        Assert.That(result, Does.StartWith($"Numbers sorted and saved to {expectedFileName}"));
        _sortAlgorithmMock.Verify(x => x.Sort(sortedArray.ToList(), request.SortAlgorithm), Times.Once);
        _fileWriterMock.Verify(x => x.WriteToFile(
            It.IsAny<string>(), FileSaveDefaults.DirecotoryName, testString), Times.Once);
    }

    [Test]
    public async Task Should_Load_Latest_File()
    {
        // Given
        _fileReaderMock.Setup(x => x.ReadLatestFile(FileSaveDefaults.DirecotoryName))
            .Returns(Task.FromResult(new LoadedFile
            {
                FileName = fileName,
                SortedList = sortedArray
            }));

        // When
        var result = await _sortService.LoadLatestFile();

        // Then
        Assert.That(result.FileName, Is.EqualTo(fileName));
        Assert.That(result.SortedList, Is.EqualTo(sortedArray));
        _fileReaderMock.Verify(x => x.ReadLatestFile(FileSaveDefaults.DirecotoryName), Times.Once);
    }

    [Test]
    public async Task Should_Throw_InvalidRequestArrayException()
    {
        // Given
        var invalidArrayString = "1, 2, 3, 5 7 11 21 31";
        var errorMessage = $"Invalid input string was submitted in the request {invalidArrayString}. " +
                $"The array string should look like this: 1 0 7 4 2";

        // When
        var exception = Assert.ThrowsAsync<InvalidRequestArrayException>(async () =>
        await _sortService.ChooseSortAndSaveNumbers(new SortRequestDTO
        {
            ArrayString = invalidArrayString,
            SortAlgorithm = AlgorithmNames.Merge
        }));

        // Then
        Assert.That(exception.Message, Is.EqualTo(errorMessage));
        _sortAlgorithmMock.Verify(x => x.Sort(
            It.IsAny<List<int>>(), It.IsAny<string>()), Times.Never);
        _fileWriterMock.Verify(x => x.WriteToFile(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Should_Throw_InvalidAlgorithmException()
    {
        // Given
        var request = new SortRequestDTO { 
            ArrayString = testString, 
            SortAlgorithm = "Invalid"         
        };

        var errorMessage = $"Invalid sort algorithm: {request.SortAlgorithm}. Available options are: ";

        // When
        var exception = Assert.ThrowsAsync<InvalidAlgorithmException>(async () => 
            await _sortService.ChooseSortAndSaveNumbers(request));

        // Then
        Assert.That(exception.Message, Does.StartWith(errorMessage));
        _sortAlgorithmMock.Verify(x => x.Sort(
            It.IsAny<List<int>>(), It.IsAny<string>()), Times.Never);
        _fileWriterMock.Verify(x => x.WriteToFile(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Should_Handle_Exception_Thrown_By_WriteToFile()
    {
        // Given
        _sortAlgorithmMock.Setup(x => x.Sort(sortedArray.ToList(), AlgorithmNames.Merge));
        _fileWriterMock.Setup(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .Throws(new IOException("Error writing to file"));

        // When
        var exception = Assert.ThrowsAsync<IOException>(async () => 
            await _sortService.MergeSortAndSaveNumbers(testString));

        // Then
        Assert.That(exception.Message, Is.EqualTo("Error writing to file"));
        _sortAlgorithmMock.Verify(x => x.Sort(sortedArray.ToList(), AlgorithmNames.Merge), Times.Once);
        _fileWriterMock.Verify(x => x.WriteToFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Should_Handle_Exception_Thrown_By_ReadLatestFile()
    {
        // Given
        _sortAlgorithmMock.Setup(x => x.Sort(sortedArray.ToList(), AlgorithmNames.Merge));
        _fileReaderMock.Setup(x => x.ReadLatestFile(It.IsAny<string>()))
            .Throws(new FileNotFoundException("No files found in the directory."));

        // When
        var exception = Assert.ThrowsAsync<FileNotFoundException>(async () =>
            await _sortService.LoadLatestFile());

        // Then
        Assert.That(exception.Message, Is.EqualTo("No files found in the directory."));
        _fileReaderMock.Verify(x => x.ReadLatestFile(It.IsAny<string>()), Times.Once);
    }
}
