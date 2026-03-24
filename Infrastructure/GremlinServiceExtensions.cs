namespace graphdb_linking_assets.Infrastructure;

using Gremlin.Net.Driver;
using Microsoft.Extensions.Options;

public static class GremlinServiceExtensions
{
    public static IHostApplicationBuilder AddGremlinClient(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<GremlinOptions>()
            .BindConfiguration("Gremlin")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<GremlinClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<GremlinOptions>>().Value;
            var config = sp.GetRequiredService<IConfiguration>();
            var server = new GremlinServer(
                $"{opts.Hostname}.gremlin.cosmos.azure.com",
                443,
                enableSsl: true,
                username: $"/dbs/{opts.Database}/colls/{opts.Collection}",
                password: config[KeyVaultSecrets.GremlinPrimaryKey]
            );
            return new GremlinClient(server);
        });

        return builder;
    }
}
