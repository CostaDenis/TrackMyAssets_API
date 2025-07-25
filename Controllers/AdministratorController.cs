using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Authorize(Roles = "Administrator")]
[Route("administrators")]
public class AdministratorController : ControllerBase
{
    private readonly IAdministratorService _administratorService;

    public AdministratorController(IAdministratorService administratorService)
    {
        _administratorService = administratorService;
    }

    [HttpGet]
    [Route("overview")]
    public IActionResult Dashboard(
        [FromServices] IUserService _userService,
        [FromServices] IAssetService _assetService,
        [FromServices] IUserAssetService _userAssetService
    )
    {
        var totalUsers = _userService.CountUser();
        var totalAssets = _assetService.CountAsset();
        var totalUserAssets = _userAssetService.CountUserAsset();

        return Ok(new
        {
            totalUsers,
            totalAssets,
            totalUserAssets
        });
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login(
        [FromBody] LoginDTO loginDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var adm = _administratorService.Login(loginDTO);

        if (adm == null)
            return Unauthorized();


        string token = _tokenService.GenerateTokenJwt(adm.Id, adm.Email, "Administrator");

        return Ok(new LoggedAdministratorViewModel
        {
            Id = adm.Id,
            Email = adm.Email,
            Token = token
        });

    }

    [HttpGet]
    [Route("users")]
    public IActionResult GetAllUsers(
        [FromQuery] int? page
    )
    {
        var usersViewModel = new List<UserViewModel>();
        var users = _administratorService.GetAllUsers(page);

        foreach (var usr in users)
        {
            usersViewModel.Add(new UserViewModel
            {
                Id = usr.Id,
                Email = usr.Email
            });
        }

        return Ok(users);
    }

    [HttpGet]
    [Route("users/{id:guid}")]
    public IActionResult GetUserById(
        [FromRoute] Guid id
    )
    {
        var user = _administratorService.GetUserById(id);

        if (user == null)
            return NotFound();


        return Ok(new UserViewModel
        {
            Id = user.Id,
            Email = user.Email
        });
    }

    [HttpPut]
    [Route("change-email")]
    public IActionResult UpdateEmail(
        [FromBody] UpdateEmailDTO updateEmailDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var administratorId = _tokenService.GetUserId(HttpContext);

        if (administratorId == null)
            return Unauthorized();

        var administrator = _administratorService.GetAdministrator(administratorId.Value);
        var result = _administratorService.UpdateEmail(administrator!, updateEmailDTO);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);

    }

    [HttpPut]
    [Route("change-password")]
    public IActionResult UpdatePassword(
   [FromBody] UpdatePasswordDTO updatePasswordDTO,
   [FromServices] ITokenService _tokenService
)
    {
        var userId = _tokenService.GetUserId(HttpContext);

        if (userId == null)
            return Unauthorized();

        var administrator = _administratorService.GetAdministrator(userId.Value);
        var result = _administratorService.UpdatePassword(administrator!, updatePasswordDTO);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);

    }

    [HttpDelete]
    [Route("users/{id:guid}")]
    public IActionResult DeleteUser(
        [FromRoute] Guid id
    )
    {
        var user = _administratorService.GetUserById(id);

        if (user == null)
            return NotFound();

        _administratorService.DeleteUser(user);

        return NoContent();
    }

}