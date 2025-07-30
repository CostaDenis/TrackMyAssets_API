using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMyAssets_API.Domain.DTOs;
using TrackMyAssets_API.Domain.Entities;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.Enums;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Route("assets")]
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
            return BadRequest(new ResultViewModel<AssetViewModel>("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency"));


        var asset = new Asset
        {
            Name = assetDTO.Name,
            Symbol = assetDTO.Symbol,
            Type = parsedType
        };

        _assetService.Create(asset);

        return Created(
                        $"/assets/{asset.Id}",
                        new ResultViewModel<AssetViewModel>(
                            new AssetViewModel
                            {
                                Name = asset.Name,
                                Symbol = asset.Symbol,
                                Type = asset.Type.ToString()
                            }
                        )
                    );
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    public IActionResult GetAll(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var assetsViewModel = new List<AssetViewModel>();
        var assets = _assetService.GetAll(page, pageSize);

        foreach (var assts in assets)
        {
            assetsViewModel.Add(new AssetViewModel
            {
                Name = assts.Name,
                Symbol = assts.Symbol!,
                Type = assts.Type.ToString()
            });
        }

        return Ok(new ResultViewModel<List<AssetViewModel>>(assetsViewModel));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    [Route("{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id
    )
    {
        var asset = _assetService.GetById(id);

        if (asset == null)
            return NotFound(new ResultViewModel<AssetViewModel>("Ativo não encontrado."));

        var data = new AssetViewModel
        {
            Name = asset.Name,
            Symbol = asset.Symbol!,
            Type = asset.Type.ToString()
        };

        return Ok(new ResultViewModel<AssetViewModel>(data));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    [Route("{name}")]
    public IActionResult GetByName(
        [FromRoute] string name
    )
    {
        var asset = _assetService.GetByName(name);

        if (asset == null)
            return NotFound(new ResultViewModel<AssetViewModel>("Ativo não encontrado."));

        var data = new AssetViewModel
        {
            Name = asset.Name,
            Symbol = asset.Symbol!,
            Type = asset.Type.ToString()
        };

        return Ok(new ResultViewModel<AssetViewModel>(data));
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    [Route("{id:guid}")]
    public IActionResult Update(
        [FromBody] AssetDTO assetDTO,
        Guid id
    )
    {
        var asset = _assetService.GetById(id);
        if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
            return BadRequest(new ResultViewModel<AssetViewModel>("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency"));

        if (asset == null)
            return NotFound(new ResultViewModel<AssetViewModel>("Ativo não encontrado."));

        asset.Name = assetDTO.Name;
        asset.Symbol = assetDTO.Symbol;
        asset.Type = parsedType;

        _assetService.Update(asset);

        var data = new AssetViewModel
        {
            Name = asset.Name,
            Symbol = asset.Symbol,
            Type = asset.Type.ToString()
        };

        return Ok(new ResultViewModel<AssetViewModel>(data));
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    [Route("{id:guid}")]
    public IActionResult Delete(
        [FromRoute] Guid id
    )
    {
        var asset = _assetService.GetById(id);

        if (asset == null)
            return NotFound(new ResultViewModel<AssetViewModel>("Ativo não encontrado."));

        _assetService.Delete(asset);

        return NotFound(new ResultViewModel<AssetViewModel>("Ativo excluído com sucesso!"));
    }

}