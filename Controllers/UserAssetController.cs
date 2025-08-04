
using System.Runtime.CompilerServices;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("users/assets")]
[Authorize(Roles = "User")]
public class UserAssetController : ControllerBase
{

    private readonly IUserAssetService _userAssetService;
    private readonly ITokenService _tokenService;

    public UserAssetController(IUserAssetService userAssetService,
                                ITokenService tokenService)
    {
        _userAssetService = userAssetService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetUserAssets()
    {
        var userId = _tokenService.GetUserId(this.HttpContext);
        var userAssets = _userAssetService.GetUserAssets(userId);
        var userAssetsViewModel = new List<UserAssetViewModel>();

        if (userAssets == null)
            return NotFound(new ResultViewModel<UserAssetViewModel>("Sem ativos no momento!"));

        foreach (var usr in userAssets)
        {
            userAssetsViewModel.Add(new UserAssetViewModel
            {
                UserId = usr.UserId,
                AssetId = usr.AssetId,
                Units = _userAssetService.GetAssetAmount(usr.AssetId, usr.UserId)
            });
        }

        return Ok(new ResultViewModel<List<UserAssetViewModel>>(userAssetsViewModel));
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetUserAssetByAssetId(
        [FromRoute] Guid id
    )
    {
        var userId = _tokenService.GetUserId(this.HttpContext);
        var userAsset = _userAssetService.GetUserAssetByAssetId(userId, id);

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

    [HttpPost]
    public IActionResult AddTransaction(
        [FromBody] UserAssetAddDTO userAssetDTO,
        IAssetService _assetService
    )
    {
        var userId = _tokenService.GetUserId(this.HttpContext);

        if (_assetService.GetById(userAssetDTO.AssetId) == null)
            return NotFound(new ResultViewModel<UserAssetViewModel>("Ativo não encontrado!"));

        try
        {
            var result = _userAssetService.AddTransaction(userAssetDTO.AssetId, userId, userAssetDTO.Units, userAssetDTO.Note);

            return Ok(new ResultViewModel<AssetTransaction>(result));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }


    }

}
