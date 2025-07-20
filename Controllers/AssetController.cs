using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Enums;
using TrackMyAssets_API.Domain.ModelsViews;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("/assets")]
public class AssetController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult CreateAsset(
        [FromBody] AssetDTO assetDTO
    )
    {
        if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
            return BadRequest("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency");


        var asset = new Asset
        {
            Name = assetDTO.Name,
            Symbol = assetDTO.Symbol,
            Type = parsedType
        };

        _assetService.Create(asset);

        return Created($"/assets/{asset.Id}", asset);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    public IActionResult GetAllAssets(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var assetsModelView = new List<AssetModelView>();
        var assets = _assetService.GetAll(page, pageSize);

        foreach (var assts in assets)
        {
            assetsModelView.Add(new AssetModelView
            {
                Name = assts.Name,
                Symbol = assts.Symbol!,
                Type = assts.Type.ToString()
            });
        }

        return Ok(assets);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    [Route("/{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id
    )
    {
        var asset = _assetService.GetById(id);

        if (asset == null)
            return NotFound();

        return Ok(new AssetModelView
        {
            Name = asset.Name,
            Symbol = asset.Symbol!,
            Type = asset.Type.ToString()
        });
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    [Route("/{name}")]
    public IActionResult GetByName(
        [FromRoute] string name
    )
    {
        var asset = _assetService.GetByName(name);

        if (asset == null)
            return NotFound();

        return Ok(new AssetModelView
        {
            Name = asset.Name,
            Symbol = asset.Symbol!,
            Type = asset.Type.ToString()
        });
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    [Route("/{id:guid}")]
    public IActionResult Update(
        [FromBody] AssetDTO assetDTO,
        Guid id
    )
    {
        var asset = _assetService.GetById(id);
        if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
        {
            return BadRequest("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency");
        }

        if (asset == null)
            return NotFound();


        asset.Name = assetDTO.Name;
        asset.Symbol = assetDTO.Symbol;
        asset.Type = parsedType;

        _assetService.Update(asset);

        return Ok(asset);
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    [Route("/{id:guid}")]
    public IActionResult Delete(
        [FromRoute] Guid id
    )
    {
        var asset = _assetService.GetById(id);

        if (asset == null)
            return NotFound();

        _assetService.Delete(asset);

        return NoContent();
    }

}