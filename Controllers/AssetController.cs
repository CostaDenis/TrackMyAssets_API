using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpGet]
    [Authorize(Roles = "Administrator, User")]
    public IActionResult GetAll(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var assetsViewModel = new List<AssetViewModel>();

        try
        {
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
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

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

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult Create(
        [FromBody] AssetDTO assetDTO
    )
    {
        if (!Enum.TryParse<EAsset>(assetDTO.Type, true, out var parsedType))
            return BadRequest(new ResultViewModel<AssetViewModel>("Tipo de ativo indisponível! As opções são: Stock, RealStateFund e Cryptocurrency"));

        try
        {
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
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Esse Nome já está sendo utilizado na aplicação!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }
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

        try
        {
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
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Esse Nome já está sendo utilizado na aplicação!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

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

        try
        {
            _assetService.Delete(asset);
            return NotFound(new ResultViewModel<string>(data: "Ativo excluído com sucesso!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }

}