
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Entities.Services;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("users/assets")]
[Authorize(Roles = "User")]
public class UserAssetController : ControllerBase
{

    private readonly IUserAssetService _userAssetService;
    private readonly HttpContext _http;
    private readonly TokenService _tokenService;

    public UserAssetController(IUserAssetService userAssetService,
                                 HttpContext http,
                                 TokenService tokenService)
    {
        _userAssetService = userAssetService;
        _http = http;
        _tokenService = tokenService;
    }

    [HttpPost]
    public IActionResult AddUnits(
        [FromBody] UserAssetAddDTO userAssetDTO,
        IAssetService assetService
    )
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        if (assetService.GetById(userAssetDTO.AssetId) == null)
            return NotFound(new ResultViewModel<UserAssetViewModel>("Ativo não encontrado!"));

        try
        {
            var result = _userAssetService.AddUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

            return Ok(new ResultViewModel<AssetTransaction>(result));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }


    }

    [HttpPut]
    public IActionResult RemoveUnits(
        [FromBody] UserAssetRemoveDTO userAssetDTO
    )
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        try
        {
            var result = _userAssetService.RemoveUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

            return Ok(new ResultViewModel<AssetTransaction>(result));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

    [HttpGet]
    public IActionResult Get()
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        var userAssets = _userAssetService.UserAssets(userId.Value);
        var userAssetsViewModel = new List<UserAssetViewModel>();

        if (userAssets == null)
            return NotFound(new ResultViewModel<UserAssetViewModel>("Sem ativos no momento!"));

        foreach (var usr in userAssets)
        {
            userAssetsViewModel.Add(new UserAssetViewModel
            {
                UserId = usr.UserId,
                AssetId = usr.AssetId,
                Units = usr.Units
            });
        }

        return Ok(new ResultViewModel<List<UserAssetViewModel>>(userAssetsViewModel));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id
    )
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized(new ResultViewModel<LoggedUserViewModel>("Acesso negado!"));

        var userAsset = _userAssetService.GetUserAssetByAssetId(userId.Value, id);

        if (userAsset == null)
            return NotFound(new ResultViewModel<UserAssetViewModel>("Sem ativos no momento!"));

        var data = new UserAssetViewModel
        {
            UserId = userAsset.UserId,
            AssetId = userAsset.AssetId,
            Units = userAsset.Units
        };

        return Ok(new ResultViewModel<UserAssetViewModel>(data));
    }
}
