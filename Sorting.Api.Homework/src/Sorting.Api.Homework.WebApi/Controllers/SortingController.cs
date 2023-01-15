using Microsoft.AspNetCore.Mvc;
using Sorting.Api.Homework.WebApi.Exceptions;
using Sorting.Api.Homework.WebApi.Models.DTO;
using Sorting.Api.Homework.WebApi.Services;

namespace Sorting.Api.Homework.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SortingController : ControllerBase
{
    private readonly ISortService _sortService;

    public SortingController(ISortService sortService)
    {
        _sortService = sortService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]    
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> OrderAndSave([FromBody] string numbers)
    {
        try
        {
            var response = await _sortService.MergeSortAndSaveNumbers(numbers);
            return new OkObjectResult(response);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case InvalidRequestArrayException:
                    {
                        return BadRequest(e.Message);
                    }                             
                default:
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                    }
            }
        }
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChooseAlgorithmAndSave([FromBody] SortRequestDTO request)
    {
        try
        {
            var response = await _sortService.ChooseSortAndSaveNumbers(request);
            return new OkObjectResult(response);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case InvalidAlgorithmException:
                case InvalidRequestArrayException:
                    {
                        return BadRequest(e.Message);
                    }
                default:
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                    }
            }
        }
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoadLatestFile()
    {
        try
        {
            var response = await _sortService.LoadLatestFile();
            return new OkObjectResult(response);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case DirectoryNotFoundException:
                case FileNotFoundException:
                    {
                        return NoContent();
                    }
                default:
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                    }
            }
        }
    }
}
