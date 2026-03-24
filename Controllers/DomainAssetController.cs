namespace graphdb_linking_assets.Controllers;

using graphdb_linking_assets.Models;
using graphdb_linking_assets.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("domain-assets")]
public class DomainAssetController(DomainAssetService service) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<DomainAsset>> GetAll() =>
        await service.GetAllDomainAssets();

    [HttpPost]
    public async Task<ActionResult<DomainAsset>> Create(DomainAsset asset)
    {
        var created = await service.CreateDomainAsset(asset);
        return CreatedAtAction(nameof(GetAll), created);
    }

    [HttpGet("{id:guid}/linked")]
    public async Task<IEnumerable<DomainAsset>> GetLinked(Guid id) =>
        await service.GetDomainAssetsLinkedTo(id);

    [HttpPost("links")]
    public async Task<IActionResult> Link(DomainAssetLink link)
    {
        await service.LinkDomainAssets(link);
        return NoContent();
    }
}
