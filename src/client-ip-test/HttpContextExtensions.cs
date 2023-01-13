using System.Net;
using Microsoft.AspNetCore.Http.Features;

namespace client_ip_test;

public static class HttpContextExtensions
{
    public static (string ipAddress, string source) GetClientIp(this HttpContext httpContext)
    {
        const string httpContextSource = "HttpContext RemoteIpAddress";
        const string realIpHeader = "X-Real-IP";
        const string forwardedForHeader = "X-Forwarded-For";

        var remoteIp = httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        var result = remoteIp?.MapToIPv4().ToString() ?? string.Empty;

        // If the IP is not loopback, then we can already return.
        if (remoteIp != null && !IPAddress.IsLoopback(remoteIp))
        {
            return (result, httpContextSource);
        }

        var headers = httpContext.Request.Headers;

        // If the resolved IP is a loopback address, then we further check if it is from proxy.
        // If the request is from a proxy (generally NGINX uses this header), get the real client IP from the X-Real-IP header
        if (headers.TryGetValue(realIpHeader, out var realIp) &&
            !string.IsNullOrWhiteSpace(realIp))
        {
            return (realIp, realIpHeader);
        }

        // If the request is from a proxy, get the client IP from X-Forwarded-For header
        if (headers.TryGetValue(forwardedForHeader, out var forwardedForIp) &&
            !string.IsNullOrWhiteSpace(forwardedForIp))
        {
            return (forwardedForIp, forwardedForHeader);
        }

        return (result, httpContextSource);
    }
}