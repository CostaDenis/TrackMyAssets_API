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

    public AssetTransactionController(IAssetTransactionService assetTransactionService)
    {
        _assetTransactionService = assetTransactionService;
    }

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] ITokenService _tokenService
    )
    {
        var userId = _tokenService.GetUserId(this.HttpContext);
        var transactions = _assetTransactionService.GetAll(userId, page, pageSize);
        var transactionsViewModel = new List<AssetTransactionViewModel>();

        if (transactions == null)
        {
            return NotFound(new ResultViewModel<AssetTransactionViewModel>("Nenhuma transação encontrada!"));
        }

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
}