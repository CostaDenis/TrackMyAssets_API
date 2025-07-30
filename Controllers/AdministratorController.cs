using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        var data = new DashBoardViewModel
        {
            TotalUsers = totalUsers,
            TotalAssets = totalAssets,
            TotalUserAssets = totalUserAssets
        };

        return Ok(new ResultViewModel<DashBoardViewModel>(data));
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


    [HttpGet]
    [Route("users")]
    public IActionResult GetAllUsers(
        [FromQuery] int? page
    )
    {
        var usersViewModel = new List<UserViewModel>();

        try
        {
            var users = _administratorService.GetAllUsers(page);

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

    [HttpPut]
    [Route("change-email")]
    public IActionResult UpdateEmail(
        [FromBody] UpdateEmailDTO updateEmailDTO,
        [FromServices] ITokenService _tokenService
    )
    {
        var administratorId = _tokenService.GetUserId(HttpContext);

        if (administratorId == null)
            return Unauthorized(new ResultViewModel<LoggedAdministratorViewModel>("Acesso negado!"));

        try
        {
            var administrator = _administratorService.GetAdministrator(administratorId.Value);
            var result = _administratorService.UpdateEmail(administrator!, updateEmailDTO);

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
            return Unauthorized(new ResultViewModel<LoggedAdministratorViewModel>("Acesso negado!"));

        try
        {
            var administrator = _administratorService.GetAdministrator(userId.Value);
            var result = _administratorService.UpdatePassword(administrator!, updatePasswordDTO);

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