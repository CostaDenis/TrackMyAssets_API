using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Authorize(Roles = "User")]
[Route(("users"))]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login(
        [FromBody] LoginDTO login,
        [FromServices] ITokenService _tokenService
    )
    {
        var user = _userService.Login(login);

        if (user == null)
            return Unauthorized();


        string token = _tokenService.GenerateTokenJwt(user.Id, user.Email, "User");

        return Ok(new LoggedUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Post(
        [FromBody] UserDTO userDTO
    )
    {
        var user = _userService.Create(userDTO.Email, userDTO.Password);

        return Created($"/users/{user.Id}", new { user.Id });
    }

    [HttpPut]
    [Route("change-email")]
    public IActionResult UpdateEmail(
        [FromBody] UpdateEmailDTO updateEmailDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var userId = _tokenService.GetUserId(HttpContext);

        if (userId == null)
            return Unauthorized();

        var user = _userService.GetById(userId.Value);
        var result = _userService.UpdateEmail(user!, updateEmailDTO);

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

        var user = _userService.GetById(userId.Value);
        var result = _userService.UpdatePassword(user!, updatePasswordDTO);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);

    }

    [HttpDelete]
    public IActionResult Delete(
        [FromServices] ITokenService _tokenService
    )
    {
        var userId = _tokenService.GetUserId(HttpContext);

        if (userId == null)
            return Unauthorized();

        var user = _userService.GetById(userId.Value);
        _userService.DeleteOwnUser(user!);

        return NoContent();
    }

}