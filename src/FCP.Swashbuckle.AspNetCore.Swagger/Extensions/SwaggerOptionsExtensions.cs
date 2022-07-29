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
        internal const string HttpHeader_Forward_Proto = "X-Forwarded-Proto";
        internal const string HttpHeader_Forward_Host = "X-Forwarded-Host";
        internal const string HttpHeader_Forward_Prefix = "X-Forwarded-Prefix";

        public static SwaggerOptions ResolveBasePathByRequestReferer(this SwaggerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.PreSerializeFilters.Add(BuildBasePathFilterByCheckRequestReferer());

            return options;
        }

        public static SwaggerOptions ResolveBasePathByRequestForward(this SwaggerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.PreSerializeFilters.Add(BuildBasePathFilterByCheckRequestForward());

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

        private static Action<OpenApiDocument, HttpRequest> BuildBasePathFilterByCheckRequestForward()
        {
            return (swaggerDoc, httpReq) =>
            {
                var forwardHost = httpReq.Headers[HttpHeader_Forward_Host].ToString();
                if (string.IsNullOrWhiteSpace(forwardHost))
                    return;
                
                var docHost = httpReq.Headers[HeaderNames.Host].ToString();
                if (string.Compare(forwardHost, docHost, true) == 0)
                    return;

                var forwardProto = httpReq.Headers[HttpHeader_Forward_Proto].ToString();
                var forwardPrefix = httpReq.Headers[HttpHeader_Forward_Prefix].ToString();

                swaggerDoc.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{forwardProto}://{forwardHost}{forwardPrefix}" }
                };
            };
        }
    }
}
