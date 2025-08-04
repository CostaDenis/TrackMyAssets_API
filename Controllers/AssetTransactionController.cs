using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using TrackMyAssets_API.Domain.Entities.Interfaces;
using TrackMyAssets_API.Domain.ViewModels;

namespace TrackMyAssets_API.Controllers;

[ApiController]
[Authorize(Roles = "User")]
[Route("asset-transactions")]
public class AssetTransactionController : ControllerBase
{
    private readonly IAssetTransactionService _assetTransactionService;
    private readonly ITokenService _tokenService;

    public AssetTransactionController(IAssetTransactionService assetTransactionService, ITokenService tokenService)
    {
        _assetTransactionService = assetTransactionService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var userId = _tokenService.GetUserId(this.HttpContext);
        var transactions = _assetTransactionService.GetAll(userId, page, pageSize);
        var transactionsViewModel = new List<AssetTransactionViewModel>();

        try
        {
            if (transactions == null)
                return NotFound(new ResultViewModel<AssetTransactionViewModel>("Nenhuma transação encontrada!"));


            foreach (var t in transactions)
            {
                transactionsViewModel.Add(new AssetTransactionViewModel
                {
                    Id = t.Id,
                    AssetId = t.AssetId,
                    UnitsChanged = t.UnitsChanged,
                    Date = t.Date,
                    Type = t.Type,
                    Note = t.Note
                });
            }

            return Ok(new ResultViewModel<List<AssetTransactionViewModel>>(transactionsViewModel));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }
    }

    [HttpGet]
    [Route("{assetId:guid}")]
    public IActionResult GetByAssetId(
        [FromRoute] Guid assetId
    )
    {
        var userId = _tokenService.GetUserId(this.HttpContext);
        var transactions = _assetTransactionService.GetByAssetId(assetId, userId);
        var transactionsViewModel = new List<AssetTransactionViewModel>();

        try
        {
            if (transactions == null)
                return NotFound(new ResultViewModel<AssetTransactionViewModel>("Nenhuma transação encontrada!"));

            foreach (var t in transactions)
            {
                transactionsViewModel.Add(new AssetTransactionViewModel
                {
                    Id = t.Id,
                    AssetId = t.AssetId,
                    UnitsChanged = t.UnitsChanged,
                    Date = t.Date,
                    Type = t.Type,
                    Note = t.Note
                });
            }

            return Ok(new ResultViewModel<List<AssetTransactionViewModel>>(transactionsViewModel));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor!"));
        }

    }
}