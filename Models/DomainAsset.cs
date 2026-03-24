namespace graphdb_linking_assets.Models;

using System.ComponentModel.DataAnnotations;

public class DomainAsset
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    public const string VertexLabel = "domain_asset";
}
