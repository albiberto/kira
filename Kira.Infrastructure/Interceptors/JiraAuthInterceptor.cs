namespace Kira.Infrastructure.Interceptors;

using System.Text;
using Microsoft.Extensions.Options;
using Options;

public class JiraAuthInterceptor : DelegatingHandler
{
    readonly string credentials;

    public JiraAuthInterceptor(IOptions<AuthOptions> options)
    {
        var login = $"{options.Value.Username}:{options.Value.Password}";
        credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new("Basic", credentials);
        return await base.SendAsync(request, cancellationToken);
    }
}