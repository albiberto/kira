namespace Kira;

using Builders;
using Comparers.Filter;
using Infrastructure.Clients;
using Infrastructure.Interceptors;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Radzen;

public static class Bootstrapper
{
    public static void AddKiraOptions(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<AuthOptions>()
            .Bind(builder.Configuration.GetSection("Kira:Auth"))
            .ValidateDataAnnotations();

        builder.Services
            .AddOptions<JiraOptions>()
            .Bind(builder.Configuration.GetSection("Kira:Jira"))
            .ValidateDataAnnotations();
    }

    public static void AddKira(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<FilterBuilder>();
        builder.Services.AddSingleton<StatusModelComparer>();
        builder.Services.AddSingleton<TypeModelComparer>();

        builder.Services.AddSingleton<JiraAuthInterceptor>();
        builder.Services
            .AddHttpClient<JiraClient>((services, client) =>
            {
                var options = services.GetRequiredService<IOptions<JiraOptions>>().Value;
                client.BaseAddress = new(options.BaseAddress);
            })
            .AddHttpMessageHandler<JiraAuthInterceptor>();
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