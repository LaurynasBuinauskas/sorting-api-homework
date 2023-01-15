using System.IO;

namespace Sorting.Api.Homework.WebApi.Tests.Unit.Readers
{
    internal class FileReaderTests
    {
        protected Mock<IFileSystem> _fileSystemMock;
        protected IFileReader _fileReader;
        protected Mock<FileBase> _fileMock;

        protected string _directory;
        
        protected const string testDirectoryName = "SortingTestResults";

        protected const string FileNotFoundExceptionMessage = "An error occurred while trying to open a file: No files found in the directory.";
        protected const string UnauthorizedAccessExceptionMessage = "An error occurred while reading the file: Attempted to perform an unauthorized operation.";
        protected const string IOExceptionMessage = "An error occurred while reading the file: I/O error occurred.";
        protected const string GenericExceptionMessage = "An error occured while reading sorted numbers: Exception of type 'System.Exception' was thrown.";
        protected const string DirectoryNotFoundExceptionMessage = "An error occurred while opening the directory: Directory not found";

        protected string[] files;

        protected const string fileContentString = "1 2 3 5 7 11 21 31 111";
        protected readonly int[] sortedArray = { 1, 2, 3, 5, 7, 11, 21, 31, 111 };

        [SetUp]
        public void Setup()
        {
            _fileMock = new Mock<FileBase>();
            _fileSystemMock = new Mock<IFileSystem>();
            _fileReader = new FileReader(_fileSystemMock.Object);
            _directory = Path.Combine(AppContext.BaseDirectory, testDirectoryName);

            files = new[] {
                "testFile1.txt", "testFile2.txt", "testFile3.txt"
            };

            _fileSystemMock.Setup(x => x.File).Returns(_fileMock.Object);


            _fileSystemMock.Setup(x => x.File.GetLastWriteTime(files[0]))
                .Returns(new DateTime(2021, 1, 1));
            _fileSystemMock.Setup(x => x.File.GetLastWriteTime(files[1]))
                .Returns(new DateTime(2022, 2, 2));
            _fileSystemMock.Setup(x => x.File.GetLastWriteTime(files[2]))
                .Returns(new DateTime(2023, 3, 3));

            _fileSystemMock.Setup(x => x.File.ReadAllTextAsync(
                files[2], CancellationToken.None))
                .ReturnsAsync(fileContentString);

            _fileSystemMock.Setup(x => x.Path
                .Combine(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_directory);
        }

        [Test]
        public async Task Should_Read_Latest_File()
        {
            // Given
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(x => x.Directory.GetFiles(_directory))
                .Returns(files);

            // When
            var result = await _fileReader.ReadLatestFile(testDirectoryName);

            // Then
            Assert.That(result.SortedList, Is.EqualTo(sortedArray));
            Assert.That(result.FileName, Is.EqualTo(files[2]));
        }

        [Test]
        public async Task Should_Throw_DirectoryNotFound()
        {
            // Given             
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(false);

            // When
            var exception = Assert.ThrowsAsync<DirectoryNotFoundException>(async () =>
                await _fileReader.ReadLatestFile(testDirectoryName));

            var message = exception.Message;

            // Then
            Assert.That(exception.Message, Is.EqualTo(DirectoryNotFoundExceptionMessage));
        }

        [Test]
        public async Task Should_Throw_FileNotFoundException()
        {
            // Given
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(x => x.Directory.GetFiles(It.IsAny<string>()))
                .Returns(new string[0]);

            // When
            var exception = Assert.ThrowsAsync<FileNotFoundException>(async () => 
                await _fileReader.ReadLatestFile(testDirectoryName));

            // Then
            Assert.That(exception.Message, Is.EqualTo(FileNotFoundExceptionMessage));
        }

        [Test]
        public async Task Should_Throw_UnauthorizedAccessException()
        {
            // Given
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(x => x.Directory.GetFiles(It.IsAny<string>()))
                .Returns(files);

            _fileMock.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(), CancellationToken.None))
                .Throws<UnauthorizedAccessException>();

            // When
            var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
                await _fileReader.ReadLatestFile(testDirectoryName));
            
            // Then
            Assert.That(exception.Message, Is.EqualTo(UnauthorizedAccessExceptionMessage));
        }

        [Test]
        public async Task Should_Throw_IOException()
        {
            // Given
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(x => x.Directory.GetFiles(It.IsAny<string>()))
                .Returns(files);

            _fileMock.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(), CancellationToken.None))
                .Throws<IOException>();

            // When
            var exception = Assert.ThrowsAsync<IOException>(async () => 
                await _fileReader.ReadLatestFile(testDirectoryName));

            // Then
            Assert.That(exception.Message, Is.EqualTo(IOExceptionMessage));
        }

        [Test]
        public async Task Should_Throw_Generic_Exception()
        {
            // Given
            _fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>())).Returns(true);
            _fileSystemMock.Setup(x => x.Directory.GetFiles(It.IsAny<string>()))
                .Returns(files);

            _fileMock.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(), CancellationToken.None))
                .Throws<Exception>();

            // When
            var exception = Assert.ThrowsAsync<Exception>(async () => 
                await _fileReader.ReadLatestFile(testDirectoryName));            

            // Then
            Assert.That(exception.Message, Is.EqualTo(GenericExceptionMessage));
        }
    }
}
