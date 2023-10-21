namespace Kira.Infrastructure.Interceptors;

using System.Text;
using Options;

public class AuthInterceptor(AuthOptions options) : DelegatingHandler
{
    readonly string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{options.Username}:{options.Password}"));

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new("Basic", credentials);
        return base.SendAsync(request, cancellationToken);
    }
}