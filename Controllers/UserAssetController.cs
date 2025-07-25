
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
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
            return Unauthorized();

        if (assetService.GetById(userAssetDTO.AssetId) == null)
            return NotFound();


        var result = _userAssetService.AddUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

        return Ok(result);
    }

    [HttpPut]
    public IActionResult RemoveUnits(
        [FromBody] UserAssetRemoveDTO userAssetDTO
    )
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized();


        var result = _userAssetService.RemoveUnits(userAssetDTO.AssetId, userId.Value, userAssetDTO.Units, userAssetDTO.Note);

        return Ok(result);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized();

        var userAssets = _userAssetService.UserAssets(userId.Value);
        var userAssetsViewModel = new List<UserAssetViewModel>();

        if (userAssets == null)
            return NotFound();

        foreach (var usr in userAssets)
        {
            userAssetsViewModel.Add(new UserAssetViewModel
            {
                UserId = usr.UserId,
                AssetId = usr.AssetId,
                Units = usr.Units
            });
        }

        return Ok(userAssets);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id
    )
    {
        var userId = _tokenService.GetUserId(_http);

        if (userId == null)
            return Unauthorized();

        var userAsset = _userAssetService.GetUserAssetByAssetId(userId.Value, id);

        if (userAsset == null)
            return NotFound();

        return Ok(new UserAssetViewModel
        {
            UserId = userAsset.UserId,
            AssetId = userAsset.AssetId,
            Units = userAsset.Units
        });
    }
}
