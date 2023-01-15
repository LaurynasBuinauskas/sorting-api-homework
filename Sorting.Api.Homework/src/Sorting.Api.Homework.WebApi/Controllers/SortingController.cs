using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sorting.Api.Homework.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SortingController : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> OrderAndSave([FromBody] string numbers)
    {
        return Ok("Not implemented");
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> LoadLatestFile()
    {
        return Ok("Not implemented");
    }
}
