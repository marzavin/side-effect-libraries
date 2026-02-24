using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SideEffect.ServiceDefaults;

public static class ConfigurationExtensions
{
    public static TConfig AddConfiguration<TConfig>(this IServiceCollection services, IConfiguration configuration, string name)
        where TConfig : class, new()
    {
        var config = new TConfig();
        configuration.GetSection(name).Bind(config);
        services.AddSingleton(config);

        return config;
    }
}
