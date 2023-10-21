namespace Kira;

using Infrastructure.Interceptors;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Radzen;

public static class Bootstrapper
{
    public static void AddKira(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<UiOptions>()
            .Bind(builder.Configuration.GetSection("Kira:UIOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
            
        var children = builder.Configuration
            .GetSection("Kira:Companies")
            .GetChildren();

        builder.Services
            .AddOptions<CompaniesOptions>()
            .Configure(options =>
            {
                options.Companies = children.Select(child =>
                {
                    var company = child.Get<CompanyOptions>()?.Identity ?? throw new NullReferenceException("Company identity cannot be null!");
                    return new KeyValuePair<string, IdentityOptions>(child.Key, company);
                }).ToDictionary();
            });
        
        foreach (var (child, index) in children.Select((child, index) => (child, index)))
        {
            builder.Services
                .AddOptions<CompanyOptions>(child.Key)
                .Bind(child)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        
            builder.Services
                .AddHttpClient(child.Key, (provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptionsSnapshot<CompanyOptions>>().Get(child.Key).Jira;
                    client.BaseAddress = new(options.BaseAddress);
                })
                .AddHttpMessageHandler(provider =>
                {
                    var options = provider.GetRequiredService<IOptionsSnapshot<CompanyOptions>>().Get(child.Key).Auth;
                    return new AuthInterceptor(options);
                });
        
            builder.Services.AddKeyedScoped<JiraService>(child.Key, (provider, _) =>
            {
                var factory = provider.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient(child.Key);
                var logger = provider.GetRequiredService<ILogger<JiraService>>();
        
                return new(client, logger);
            });
        }
    }

    public static void AddRadzen(this WebApplicationBuilder builder)
    {
        builder.Services.AddRadzenComponents();

        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();
    }
}