namespace graphdb_linking_assets.Repositories;

using graphdb_linking_assets.Models;
using Gremlin.Net.Driver;

public class GraphTestRepository(GremlinClient gremlinClient)
{
    public async Task<DomainAsset> AddVertexAsync(DomainAsset asset)
    {
        await gremlinClient.SubmitAsync<dynamic>(
            $"g.addV('{DomainAsset.VertexLabel}')" +
            $".property('id', '{asset.Id}')" +
            $".property('assetId', '{asset.Id}')" +
            $".property('name', '{asset.Name}')" +
            $".property('tenantId', 'default')");
        return asset;
    }

    public async Task AddEdgeAsync(DomainAssetLink link)
    {
        await gremlinClient.SubmitAsync<dynamic>(
            $"g.V('{link.SourceId}').addE('{DomainAssetLink.EdgeLabel}').to(g.V('{link.TargetId}'))");
    }

    public async Task<IEnumerable<DomainAsset>> GetAllDomainAssetsAsync()
    {
        var results = await gremlinClient.SubmitAsync<dynamic>(
            $"g.V().hasLabel('{DomainAsset.VertexLabel}').project('id','name').by('assetId').by('name')");

        return results.Select(v => new DomainAsset
        {
            Id = Guid.Parse((string)v["id"]),
            Name = (string)v["name"]
        });
    }

    public async Task<IEnumerable<DomainAsset>> GetLinkedDomainAssetsAsync(Guid sourceId)
    {
        var results = await gremlinClient.SubmitAsync<dynamic>(
            $"g.V('{sourceId}').both('{DomainAssetLink.EdgeLabel}').project('id','name').by('assetId').by('name')");

        return results.Select(v => new DomainAsset
        {
            Id = Guid.Parse((string)v["id"]),
            Name = (string)v["name"]
        });
    }
}
