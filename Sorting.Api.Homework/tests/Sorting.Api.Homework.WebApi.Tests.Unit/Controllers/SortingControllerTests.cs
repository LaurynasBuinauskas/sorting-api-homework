using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sorting.Api.Homework.WebApi.Controllers;
using Sorting.Api.Homework.WebApi.Models.DTO;
using Sorting.Api.Homework.WebApi.Services;

namespace Sorting.Api.Homework.WebApi.Tests.Unit.Controllers;

internal class SortingControllerTests
{
    protected SortingController _controller;
    protected Mock<ISortService> _mockService;    

    protected const string arrayString = "1 8 3 2 4 111";
    protected readonly int[] fileContent = { 1, 2, 3, 5, 7};

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<ISortService>();
        _controller = new SortingController(_mockService.Object);
    }

    #region OrderAndSave Tests

    [Test]
    public async Task OrderAndSave_Should_Return_Ok()
    {
        // Given       
        _mockService.Setup(x => x.MergeSortAndSaveNumbers(It.IsAny<string>()))
            .ReturnsAsync("Numbers sorted and saved to file");

        // When
        var result = await _controller.OrderAndSave(arrayString);

        // Then
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(((OkObjectResult)result).Value, Is.EqualTo("Numbers sorted and saved to file"));
    }

    [Test]
    public async Task OrderAndSave_Should_Return_BadRequest()
    {
        // Given 
        var invalidArrayString = "7, 1, 2, 3";
        _mockService.Setup(x => x.MergeSortAndSaveNumbers(It.IsAny<string>()))
            .ThrowsAsync(new InvalidRequestArrayException("Invalid input string was submitted in the request"));

        // When
        var result = await _controller.OrderAndSave(invalidArrayString);

        // Then
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Invalid input string was submitted in the request"));
    }

    [Test]
    public async Task OrderAndSave_Should_Return_InternalServerError()
    {
        // Given
        _mockService.Setup(x => x.MergeSortAndSaveNumbers(It.IsAny<string>()))
            .ThrowsAsync(new Exception("An error occurred"));

        // When
        var result = await _controller.OrderAndSave(arrayString);

        // Then
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(((ObjectResult)result).Value, Is.EqualTo("An error occurred"));
    }

    #endregion

    #region ChooseAlgorithmAndSave Tests

    [Test]
    public async Task ChooseAlgorithmAndSave_Should_Return_OK()
    {
        // Given
        _mockService.Setup(x => x.ChooseSortAndSaveNumbers(It.IsAny<SortRequestDTO>()))
            .ReturnsAsync("Numbers sorted and saved to file");

        // When
        var result = await _controller.ChooseAlgorithmAndSave(
            new SortRequestDTO { ArrayString = arrayString, 
                SortAlgorithm = "MergeSort" 
            });

        // Then
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(((OkObjectResult)result).Value, Is.EqualTo("Numbers sorted and saved to file"));
    }

    [Test]
    public async Task ChooseAlgorithmAndSave_Should_Return_BadRequest_Because_Of_Invalid_Array_String()
    {
        // Given         
        _mockService.Setup(x => x.ChooseSortAndSaveNumbers(It.IsAny<SortRequestDTO>()))
            .ThrowsAsync(new InvalidRequestArrayException("Invalid input string was submitted in the request"));

        // When
        var result = await _controller.ChooseAlgorithmAndSave(
            new SortRequestDTO
            {
                ArrayString = arrayString,
                SortAlgorithm = "MergeSort"
            });

        // Then
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Invalid input string was submitted in the request"));
    }

    [Test]
    public async Task ChooseAlgorithmAndSave_Should_Return_BadRequest_Because_Of_Invalid_Algorithm()
    {
        // Given 
        _mockService.Setup(x => x.ChooseSortAndSaveNumbers(It.IsAny<SortRequestDTO>()))
            .ThrowsAsync(new InvalidAlgorithmException("Invalid sorting algorithm was chosen"));

        // When
        var result = await _controller.ChooseAlgorithmAndSave(
            new SortRequestDTO
            {
                ArrayString = arrayString,
                SortAlgorithm = "Invalid"
            });

        // Then
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        Assert.That(((BadRequestObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Invalid sorting algorithm was chosen"));
    }

    [Test]
    public async Task ChooseAlgorithmAndSave_Should_Return_InternalServerError()
    {
        // Given
        _mockService.Setup(x => x.ChooseSortAndSaveNumbers(It.IsAny<SortRequestDTO>()))
            .ThrowsAsync(new Exception("An error occurred"));

        // When
        var result = await _controller.ChooseAlgorithmAndSave(
            new SortRequestDTO 
            {
                ArrayString = arrayString,
                SortAlgorithm = "MergeSort" 
            });

        // Then
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(((ObjectResult)result).Value, Is.EqualTo("An error occurred"));
    }

    #endregion

    #region LoadLatestFile Tests

    [Test]
    public async Task LoadLatestFile_Should_Return_Ok()
    {
        // Given
        var expectedResult = new LoadedFile 
        {
            FileName = "file.txt",
            SortedList = fileContent
        };

        _mockService.Setup(x => x.LoadLatestFile()).ReturnsAsync(expectedResult);

        // When
        var result = await _controller.LoadLatestFile();

        // Then
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task LoadLatestFile_Should_Return_NoContent_Because_Of_FileNotFound()
    {
        // Given
        _mockService.Setup(x => x.LoadLatestFile())
            .ThrowsAsync(new FileNotFoundException("File not found"));

        // When
        var result = await _controller.LoadLatestFile();

        // Then
        Assert.IsInstanceOf<NoContentResult>(result);        
        Assert.That(((NoContentResult)result).StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    [Test]
    public async Task LoadLatestFile_Should_Return_InternalServerError()
    {
        // Given
        _mockService.Setup(x => x.LoadLatestFile())
            .ThrowsAsync(new Exception("An error occurred"));

        // When
        var result = await _controller.LoadLatestFile();

        // Then
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(((ObjectResult)result).Value, Is.EqualTo("An error occurred"));
    }

    [Test]
    public async Task LoadLatestFile_Should_Return_Because_Of_DirectoryNotFound()
    {
        // Given
        _mockService.Setup(x => x.LoadLatestFile())
            .ThrowsAsync(new DirectoryNotFoundException());

        // When
        var result = await _controller.LoadLatestFile();

        // Then
        Assert.IsInstanceOf<NoContentResult>(result);
        Assert.That(((NoContentResult)result).StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    #endregion
}
