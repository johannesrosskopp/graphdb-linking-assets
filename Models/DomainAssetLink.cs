namespace graphdb_linking_assets.Models;

public class DomainAssetLink
{
    public Guid SourceId { get; set; }
    public Guid TargetId { get; set; }

    public const string EdgeLabel = "links_to";
}
