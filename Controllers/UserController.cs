using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ModelsViews;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route(("/users"))]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("/login")]
    public IActionResult Login(
        [FromBody] LoginDTO login,
        [FromServices] TokenService tokenService
    )
    {
        var user = _userService.Login(login);

        if (user == null)
            return Unauthorized();


        string token = tokenService.GenerateTokenJwt(user.Id, user.Email, "User");

        return Ok(new LoggedUserModelView
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
    [Authorize(Roles = "User")]
    public IActionResult Put(
        [FromBody] UserDTO userDTO,
        HttpContext http,
        [FromServices] TokenService tokenService
    )
    {
        var userId = tokenService.GetUserId(http);

        if (userId == null)
            return Unauthorized();

        var user = _userService.GetById(userId.Value);

        user.Email = userDTO.Email;
        user.Password = userDTO.Password;

        _userService.Update(user);

        return Ok(user);
    }

    [HttpDelete]
    [Authorize(Roles = "User")]
    public IActionResult Delete(
        HttpContext http,
        [FromServices] TokenService tokenService
    )
    {
        var userId = tokenService.GetUserId(http);

        if (userId == null)
            return Unauthorized();

        var user = _userService.GetById(userId.Value);
        _userService.DeleteOwnUser(user);

        return NoContent();
    }
}