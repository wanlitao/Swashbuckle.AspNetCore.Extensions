using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerOptionsExtensions
    {
        public static SwaggerOptions ResolveBasePathByRequestReferer(this SwaggerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.PreSerializeFilters.Add(BuildBasePathFilterByCheckRequestReferer());

            return options;
        }

        private static Action<OpenApiDocument, HttpRequest> BuildBasePathFilterByCheckRequestReferer()
        {
            return (swaggerDoc, httpReq) =>
            {
                var refererUrl = httpReq.Headers[HeaderNames.Referer].ToString();
                if (string.IsNullOrWhiteSpace(refererUrl))
                    return;

                var referer = new Uri(refererUrl);
                var docHost = httpReq.Headers[HeaderNames.Host].ToString();
                if (string.Compare($"{referer.Host}:{referer.Port}", docHost, true) == 0)
                    return;

                var refererPrefixPath = referer.AbsolutePath.Substring(0, referer.AbsolutePath.IndexOf("/swagger"));

                swaggerDoc.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{referer.Scheme}://{referer.Host}:{referer.Port}{refererPrefixPath}" }
                };
            };
        }
    }
}
