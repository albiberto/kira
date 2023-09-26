namespace Kira.Controllers;

using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

public class ReportController : Controller
{
    [HttpGet("/__ssrsreport")]
    public async Task Get(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            using var httpClient = CreateHttpClient();
            var responseMessage = await ForwardRequest(httpClient, Request, url);

            CopyResponseHeaders(responseMessage, Response);

            await WriteResponse(Request, url, responseMessage, Response, false);
        }
    }

    [Route("/ssrsproxy/{*url}")]
    public async Task Proxy()
    {
        var urlToReplace = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase}/{"ssrsproxy"}/";
        var requestedUrl = Request.GetDisplayUrl()
            .Replace(urlToReplace, "", StringComparison.InvariantCultureIgnoreCase);
        var reportServerIndex = requestedUrl.IndexOf("/ReportServer", StringComparison.InvariantCultureIgnoreCase);
        if (reportServerIndex == -1)
            reportServerIndex = requestedUrl.IndexOf("/Reports", StringComparison.InvariantCultureIgnoreCase);
        var reportUrlParts = requestedUrl.Substring(0, reportServerIndex).Split('/');

        var url = $"{reportUrlParts[0]}://{reportUrlParts[1]}:{reportUrlParts[2]}{requestedUrl.Substring(reportServerIndex, requestedUrl.Length - reportServerIndex)}";

        using var httpClient = CreateHttpClient();
        var responseMessage = await ForwardRequest(httpClient, Request, url);

        CopyResponseHeaders(responseMessage, Response);

        if (Request.Method == "POST") await WriteResponse(Request, url, responseMessage, Response, true);
        else
        {
            if (responseMessage.Content.Headers.ContentType is { MediaType: "text/html" })
                await WriteResponse(Request, url, responseMessage, Response, false);
            else
                await using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
                    await responseStream.CopyToAsync(Response.Body, 81920, HttpContext.RequestAborted);
        }
    }

    static HttpClient CreateHttpClient()
    {
        var httpClientHandler = new HttpClientHandler();

        httpClientHandler.AllowAutoRedirect = true;
        httpClientHandler.UseDefaultCredentials = true;

        if (httpClientHandler.SupportsAutomaticDecompression)
            httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        return new(httpClientHandler);
    }

    static async Task<HttpResponseMessage> ForwardRequest(HttpClient httpClient, HttpRequest currentRequest, string url)
    {
        var proxyRequestMessage = new HttpRequestMessage(new(currentRequest.Method), url);

        foreach (var header in currentRequest.Headers)
            if (header.Key != "Host")
                proxyRequestMessage.Headers.TryAddWithoutValidation(header.Key, new string[] { header.Value! });

        if (currentRequest.Method != "POST") return await httpClient.SendAsync(proxyRequestMessage);

        using (var stream = new MemoryStream())
        {
            await currentRequest.Body.CopyToAsync(stream);
            stream.Position = 0;

            var body = await new StreamReader(stream).ReadToEndAsync();
            proxyRequestMessage.Content = new StringContent(body);

            if (!body.Contains("AjaxScriptManager")) return await httpClient.SendAsync(proxyRequestMessage);

            proxyRequestMessage.Content.Headers.Remove("Content-Type");
            proxyRequestMessage.Content.Headers.Add("Content-Type", new[] { currentRequest.ContentType });
        }

        return await httpClient.SendAsync(proxyRequestMessage);
    }

    static void CopyResponseHeaders(HttpResponseMessage responseMessage, HttpResponse response)
    {
        response.StatusCode = (int)responseMessage.StatusCode;
        foreach (var header in responseMessage.Headers) response.Headers[header.Key] = header.Value.ToArray();

        foreach (var header in responseMessage.Content.Headers) response.Headers[header.Key] = header.Value.ToArray();

        response.Headers.Remove("transfer-encoding");
    }

    static async Task WriteResponse(HttpRequest currentRequest, string url, HttpResponseMessage responseMessage, HttpResponse response, bool isAjax)
    {
        var result = await responseMessage.Content.ReadAsStringAsync();

        var reportServer = url.Contains("/ReportServer/", StringComparison.InvariantCultureIgnoreCase)
            ? "ReportServer"
            : "Reports";

        var reportUri = new Uri(url);
        var proxyUrl = $"{currentRequest.Scheme}://{currentRequest.Host.Value}{currentRequest.PathBase}/ssrsproxy/{reportUri.Scheme}/{reportUri.Host}/{reportUri.Port}";

        if (isAjax && result.IndexOf("|", StringComparison.Ordinal) != -1)
        {
            var builder = new StringBuilder();

            var index = 0;

            while (index < result.Length)
            {
                var delimiterIndex = result.IndexOf("|", index, StringComparison.Ordinal);
                if (delimiterIndex == -1) break;
                var length = int.Parse(result.Substring(index, delimiterIndex - index));
                if (length % 1 != 0) break;
                index = delimiterIndex + 1;
                delimiterIndex = result.IndexOf("|", index, StringComparison.Ordinal);
                if (delimiterIndex == -1) break;
                var type = result.Substring(index, delimiterIndex - index);
                index = delimiterIndex + 1;
                delimiterIndex = result.IndexOf("|", index, StringComparison.Ordinal);
                if (delimiterIndex == -1) break;
                var id = result.Substring(index, delimiterIndex - index);
                index = delimiterIndex + 1;
                if (index + length >= result.Length) break;
                var content = result.Substring(index, length);
                index += length;
                if (result.Substring(index, 1) != "|") break;
                index++;

                content = content.Replace($"/{reportServer}/", $"{proxyUrl}/{reportServer}/",
                    StringComparison.InvariantCultureIgnoreCase);
                content = content.Replace(content.Contains("./ReportViewer.aspx", StringComparison.InvariantCultureIgnoreCase)
                        ? "./ReportViewer.aspx"
                        : "ReportViewer.aspx", $"{proxyUrl}/{reportServer}/Pages/ReportViewer.aspx", StringComparison.InvariantCultureIgnoreCase);

                builder.Append($"{content.Length}|{type}|{id}|{content}|");
            }

            result = builder.ToString();
        }
        else
        {
            result = result.Replace($"/{reportServer}/", $"{proxyUrl}/{reportServer}/",
                StringComparison.InvariantCultureIgnoreCase);

            result = result.Replace(result.Contains("./ReportViewer.aspx", StringComparison.InvariantCultureIgnoreCase)
                    ? "./ReportViewer.aspx"
                    : "ReportViewer.aspx", $"{proxyUrl}/{reportServer}/Pages/ReportViewer.aspx", StringComparison.InvariantCultureIgnoreCase);
        }

        response.Headers.Remove("Content-Length");
        response.Headers.Add("Content-Length", new[] { Encoding.UTF8.GetByteCount(result).ToString() });

        await response.WriteAsync(result);
    }
}