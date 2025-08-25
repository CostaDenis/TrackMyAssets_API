using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities.DTOs;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.Exceptions;
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
    [Route("dashboard")]
    public IActionResult GetDataDashboard()
    {
        var data = _administratorService.GetDataDashboard();
        var viewModelDashBoard = new DashBoardViewModel
        {
            TotalUsers = data[0],
            TotalAssets = data[1],
            TotalUserAssets = data[2]
        };

        return Ok(new ResultViewModel<DashBoardViewModel>(viewModelDashBoard));
    }

    [HttpGet]
    [Route("users")]
    public IActionResult GetAllUsers(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var usersViewModel = new List<UserViewModel>();

        try
        {
            var users = _administratorService.GetAllUsers(page, pageSize);

            foreach (var usr in users)
            {
                usersViewModel.Add(new UserViewModel
                {
                    Id = usr.Id,
                    Email = usr.Email
                });
            }

            return Ok(new ResultViewModel<List<UserViewModel>>(usersViewModel));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

    [HttpGet]
    [Route("users/{id:guid}")]
    public IActionResult GetUserById(
        [FromRoute] Guid id
    )
    {
        var user = _administratorService.GetUserById(id);

        if (user == null)
            return NotFound(new ResultViewModel<UserViewModel>("Usuário não encontrado!"));

        var data = new UserViewModel
        {
            Id = user.Id,
            Email = user.Email
        };

        return Ok(new ResultViewModel<UserViewModel>(data));
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
            return Unauthorized(new ResultViewModel<LoggedAdministratorViewModel>("Acesso negado!"));

        string token = _tokenService.GenerateTokenJwt(adm.Id, adm.Email, "Administrator");

        var data = new LoggedAdministratorViewModel
        {
            Id = adm.Id,
            Email = adm.Email,
            Token = token
        };

        return Ok(new ResultViewModel<LoggedAdministratorViewModel>(data));
    }

    [HttpPut]
    [Route("change-password")]
    public IActionResult Update(
        [FromBody] UpdatePasswordDTO updatePasswordDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var admId = _tokenService.GetUserId(this.HttpContext);

        try
        {
            var administrator = _administratorService.GetAdministrator(admId);
            _administratorService.UpdatePassword(administrator!, updatePasswordDTO);
            return Ok(new ResultViewModel<string>(data: "Atualização da senha concluída com sucesso!"));
        }
        catch (InvalidPasswordException ex)
        {
            return StatusCode(400, new ResultViewModel<string>(ex.Message));
        }
        catch (PasswordConfirmationMismatchException ex)
        {
            return StatusCode(400, new ResultViewModel<string>(ex.Message));
        }
        catch (PasswordReuseException ex)
        {
            return StatusCode(400, new ResultViewModel<string>(ex.Message));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

    [HttpDelete]
    [Route("users/{id:guid}")]
    public IActionResult DeleteUser(
        [FromRoute] Guid id
    )
    {
        var user = _administratorService.GetUserById(id);

        if (user == null)
            return NotFound(new ResultViewModel<UserViewModel>("Usuário não encontrado!"));

        try
        {
            _administratorService.DeleteUser(user);

            return Ok(new ResultViewModel<UserViewModel>("Usuário excluído com sucesso!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

}