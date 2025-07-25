using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ModelsViews;

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
    public IActionResult Put(
        [FromBody] UserDTO userDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var userId = _tokenService.GetUserId(HttpContext);

        if (userId == null)
            return Unauthorized();

        var user = _userService.GetById(userId.Value);

        user.Email = userDTO.Email;
        user.Password = userDTO.Password;

        _userService.Update(user);

        return Ok(user);
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
        _userService.DeleteOwnUser(user);

        return NoContent();
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

        if (!_userService.VerifyPassword(user, user.Password, updatePasswordDTO.CurrentPassword))
            return BadRequest("Verificação falha da senha atual!");

        if (updatePasswordDTO.NewPassword != updatePasswordDTO.NewPasswordConfirmation)
            return BadRequest("Confirmação falha da nova senha!");

        if (_userService.VerifyPassword(user, user.Password, updatePasswordDTO.NewPassword))
            return BadRequest("A nova senha não pode ser igual a antiga senha!");

        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, updatePasswordDTO.NewPassword);

        try
        {
            _userService.Update(user);
            return Ok("Senha atualizada com sucesso!");
        }
        catch
        {
            return StatusCode(500, "Falha interna no servidor!");
        }

    }
}