namespace Kira;

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
            .AddOptions<BoardOptions.My>()
            .Bind(builder.Configuration.GetSection("Kira:Boards:My"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<BoardOptions.Customer>()
            .Bind(builder.Configuration.GetSection("Kira:Boards:Customer"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static void AddKira(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<MyAuthInterceptor>();
        builder.Services.AddScoped<CustomerAuthInterceptor>();
        
        builder.Services
            .AddHttpClient("My",(services, client) =>
            {
                var options = services.GetRequiredService<IOptions<BoardOptions.My>>().Value.Jira;
                client.BaseAddress = new(options.BaseAddress);
            })
            .AddHttpMessageHandler<MyAuthInterceptor>();
       
        builder.Services
            .AddHttpClient("Customer", (services, client) =>
            {
                var options = services.GetRequiredService<IOptions<BoardOptions.Customer>>().Value.Jira;
                client.BaseAddress = new(options.BaseAddress);
            })
            .AddHttpMessageHandler<CustomerAuthInterceptor>();

        builder.Services.AddScoped<JiraClient.My>();
        builder.Services.AddScoped<JiraClient.Customer>();
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