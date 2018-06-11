using Microsoft.Net.Http.Headers;
using System;

namespace Swashbuckle.AspNetCore.Swagger
{
    public static class SwaggerOptionsExtensions
    {
        public static SwaggerOptions ResolveBasePathByRequest(this SwaggerOptions options)
        {
            return ResolveBasePathByRequest(options, "swagger");
        }

        public static SwaggerOptions ResolveBasePathByRequest(this SwaggerOptions options,
            string swaggerRoutePrefix)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(swaggerRoutePrefix))
                throw new ArgumentNullException(nameof(swaggerRoutePrefix));

            options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                var referer = new Uri(httpReq.Headers[HeaderNames.Referer].ToString());
                var docHost = httpReq.Headers[HeaderNames.Host].ToString();
                if (string.Compare($"{referer.Host}:{referer.Port}", docHost, true) != 0)
                {
                    swaggerDoc.BasePath = referer.AbsolutePath.Substring(0, referer.AbsolutePath.IndexOf($"/{swaggerRoutePrefix}"));
                }
            });

            return options;
        }
    }
}
