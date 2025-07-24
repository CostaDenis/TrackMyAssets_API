using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("")]
[AllowAnonymous]
public class HealthController : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Ok",
            time = DateTime.UtcNow
        });
    }
}