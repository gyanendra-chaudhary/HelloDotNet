using HMAC_Authentication.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace HMAC_Authentication.Middlewares
{
    public class HMACAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        private static readonly TimeSpan NonceExpiry = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan TimestampTolerance = TimeSpan.FromMinutes(5);

        public HMACAuthenticationMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }
            if (!authHeader.ToString().StartsWith("HMAC", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }
            var tokenParts = authHeader.ToString().Substring("HMAC".Length).Trim().Split('|');

            if (tokenParts.Length != 4)

            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid HMAC format");
                return;
            }

            var clientId = tokenParts[0];
            var token = tokenParts[1];
            var nonce = tokenParts[2];
            var timestamp = tokenParts[3];

            if (!ClientSecrets.Secrets.TryGetValue(clientId, out var secretKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid client ID");
                return;
            }
            if (!DateTimeOffset.TryParse(timestamp, out var requestTime) || Math.Abs((DateTimeOffset.UtcNow - requestTime).TotalMinutes) > TimestampTolerance.TotalMinutes)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid or expired timestamp");
                return;
            }
            if (!AddNonce(nonce))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Nonce already used");
                return;
            }

            var requestBody = string.Empty;
            if (context.Request.Method == HttpMethod.Post.Method || context.Request.Method == HttpMethod.Put.Method)
            {
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            var isValid = ValidateToken(token, nonce, timestamp, context.Request, requestBody, secretKey);
            if (!isValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid HMAC token");
                return;
            }

            await _next(context);

        }


        private bool AddNonce(string nonce)
        {
            if (_memoryCache.TryGetValue(nonce, out _))
            {
                return false;
            }
            var cacheEntryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(NonceExpiry);
            _memoryCache.Set(nonce, cacheEntryOption);
            return true;
        }

        private bool ValidateToken(string token, string nonce, string timestamp, HttpRequest request, string requestBody, string secretKey)
        {
            var path = Convert.ToString(request.Path);
            var requestContent = new StringBuilder()
                .Append(request.Method.ToUpper())
                .Append(path.ToUpper())
                .Append(nonce)
                .Append(timestamp);
            if (request.Method == HttpMethod.Post.Method || request.Method == HttpMethod.Put.Method)
            {
                requestContent.Append(requestBody);
            }

            var secretBytes = Encoding.UTF8.GetBytes(secretKey);
            var requestBytes = Encoding.UTF8.GetBytes(requestContent.ToString());

            using var hmac = new HMACSHA256(secretBytes);
            var computedHash = hmac.ComputeHash(requestBytes);
            var computedToken = Convert.ToBase64String(computedHash);
            return token == computedToken;
        }
    }
}
