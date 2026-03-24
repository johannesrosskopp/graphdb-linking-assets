namespace graphdb_linking_assets.Services;

using graphdb_linking_assets.Models;
using graphdb_linking_assets.Repositories;

public class DomainAssetService(GraphTestRepository repository)
{
    public Task<DomainAsset> CreateDomainAsset(DomainAsset asset) =>
        repository.AddVertexAsync(asset);

    public Task LinkDomainAssets(DomainAssetLink link) =>
        repository.AddEdgeAsync(link);

    public Task<IEnumerable<DomainAsset>> GetAllDomainAssets() =>
        repository.GetAllDomainAssetsAsync();

    public Task<IEnumerable<DomainAsset>> GetDomainAssetsLinkedTo(Guid sourceId) =>
        repository.GetLinkedDomainAssetsAsync(sourceId);
}
