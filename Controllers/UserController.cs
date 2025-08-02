using Blog.ViewModels;
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

        var data = new LoggedUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        };

        return Ok(new ResultViewModel<LoggedUserViewModel>(data));
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Create(
        [FromBody] UserDTO userDTO
    )
    {
        try
        {
            var user = _userService.Create(userDTO.Email, userDTO.Password);

            return Created(
                            $"/users/{user.Id}",
                            new UserViewModel
                            {
                                Id = user.Id,
                                Email = user.Email
                            });
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Esse email já está sendo utilizado na aplicação!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

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
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        try
        {
            var user = _userService.GetById(userId.Value);
            var result = _userService.UpdateEmail(user!, updateEmailDTO);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Esse email já está sendo utilizado na aplicação!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

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
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        try
        {
            var user = _userService.GetById(userId.Value);
            var result = _userService.UpdatePassword(user!, updatePasswordDTO);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

    [HttpDelete]
    public IActionResult DeleteOwnUser(
        [FromServices] ITokenService _tokenService
    )
    {
        var userId = _tokenService.GetUserId(HttpContext);

        if (userId == null)
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        try
        {
            var user = _userService.GetById(userId.Value);
            _userService.DeleteOwnUser(user!);

            return Ok(new ResultViewModel<UserViewModel>("Usuário excluído com sucesso!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

}