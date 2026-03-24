namespace graphdb_linking_assets.Infrastructure;

public class GremlinOptions
{
    public required string Hostname { get; set; }
    public required string Database { get; set; }
    public required string Collection { get; set; }
}
