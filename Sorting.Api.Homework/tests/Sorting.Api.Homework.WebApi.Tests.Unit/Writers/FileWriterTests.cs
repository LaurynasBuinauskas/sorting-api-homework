using Sorting.Api.Homework.WebApi.InputOutput.Writers;
using System.IO;
using System.IO.Abstractions;

namespace Sorting.Api.Homework.WebApi.Tests.Unit.Writers;

internal class FileWriterTests
{
    protected Mock<IFileSystem> _fileSystemMock;
    protected IFileWriter _fileWriter;
    protected Mock<FileBase> _fileMock;    

    protected const string testFileName = "testFile.txt";
    protected const string testDirectoryName = "SortingTestResults";
    protected const string testFileContent = "1 2 3 5 7";

    protected const string UnauthorizedAccessExceptionMessage = "An error occurred while writing the submitted to a file: Attempted to perform an unauthorized operation.";
    protected const string IOExceptionMessage = "An error occurred while writing the submitted to a file: I/O error occurred.";
    protected const string GenericExceptionMessage = "An error occured while saving the submitted numbers: Exception of type 'System.Exception' was thrown.";


    [SetUp]
    public void SetUp()
    {
        _fileMock = new Mock<FileBase>();
        _fileSystemMock = new Mock<IFileSystem>();
        _fileWriter = new FileWriter(_fileSystemMock.Object);        

        _fileSystemMock.Setup(x => x.File).Returns(_fileMock.Object);

        _fileSystemMock.Setup(x => x.Path.Combine(It.IsAny<string>(), It.IsAny<string>()))
        .Returns((string a, string b) => $"{a}\\{b}");
    }

    [Test]
    public async Task Should_Write_To_File()
    {
        // Given
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);        

        // When
        await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent);

        // Then
        _fileMock.Verify(x => x.WriteAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            CancellationToken.None), Times.Once());
    }

    [Test]
    public async Task Should_Create_Directory_When_It_Does_Not_Exist()
    {
        // Given              
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(false);        

        // When
        await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent);

        // Then
        _fileSystemMock.Verify(
            x => x.Directory.CreateDirectory(
                It.Is<string>(s => s == $"{AppContext.BaseDirectory}\\{testDirectoryName}")),
                Times.Once());
    }

    [Test]
    public async Task Should_Not_Create_Directory_When_It_Exists()
    {
        // Given           
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
        
        // When
        await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent);

        // Then
        _fileSystemMock.Verify(x => x.Directory.CreateDirectory(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public async Task Should_Throw_UnauthorizedAccessException()
    {
        // Given
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
        
        _fileSystemMock.Setup(x => x.File.WriteAllTextAsync(
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            CancellationToken.None))
            .ThrowsAsync(new UnauthorizedAccessException());

        // When/Then
        var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
            await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent));        

        Assert.That(exception.Message, Is.EqualTo(UnauthorizedAccessExceptionMessage));
    }

    [Test]
    public async Task Should_Throw_IOException()
    {
        // Given
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);

        _fileSystemMock.Setup(x => x.File.WriteAllTextAsync(
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            CancellationToken.None))
            .ThrowsAsync(new IOException());

        // When/Then
        var exception = Assert.ThrowsAsync<IOException>(async () => 
            await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent));        

        Assert.That(exception.Message, Is.EqualTo(IOExceptionMessage));
    }

    [Test]
    public async Task Should_Throw_Exception()
    {
        // Given
        _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);

        _fileSystemMock.Setup(x => x.File.WriteAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            CancellationToken.None))
            .ThrowsAsync(new Exception());

        // When/Then
        var exception = Assert.ThrowsAsync<Exception>(async () => 
            await _fileWriter.WriteToFile(testFileName, testDirectoryName, testFileContent));        

        Assert.That(exception.Message, Is.EqualTo(GenericExceptionMessage));
    }
}
