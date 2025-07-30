using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("")]
[AllowAnonymous]
public class HealthController : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        var data = new HealthViewModel
        {
            Status = "Ok",
            Time = DateTime.UtcNow
        };

        return Ok(new ResultViewModel<HealthViewModel>(data));
    }
}