namespace Kira.Infrastructure.Interceptors;

using System.Text;
using Microsoft.Extensions.Options;
using Options;

public class CustomerAuthInterceptor : DelegatingHandler
{
    readonly string credentials;

    public CustomerAuthInterceptor(IOptions<BoardOptions.Customer> options)
    {
        var login = $"{options.Value.Auth.Username}:{options.Value.Auth.Password}";
        credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new("Basic", credentials);
        return await base.SendAsync(request, cancellationToken);
    }
}