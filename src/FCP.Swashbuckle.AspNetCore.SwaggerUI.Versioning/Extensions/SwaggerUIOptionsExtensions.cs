using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerUIOptionsExtensions
    {
        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider)
        {
            return VersioningSwaggerEndpoints(options, provider, false);
        }

        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider, bool urlRelative)
        {
            return VersioningSwaggerEndpoints(options, provider, "{0} Docs", urlRelative);
        }

        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider, string docDescriptionFormat)
        {
            return VersioningSwaggerEndpoints(options, provider, docDescriptionFormat, false);
        }

        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider, string docDescriptionFormat, bool urlRelative)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (string.IsNullOrWhiteSpace(docDescriptionFormat))
                throw new ArgumentNullException(nameof(docDescriptionFormat));

            foreach (var description in provider.ApiVersionDescriptions)
            {
                var endPointUrl = $"{description.GroupName}/swagger.json";
                if (!urlRelative)
                {
                    endPointUrl = $"/{options.RoutePrefix}/{endPointUrl}";
                }

                options.SwaggerEndpoint(endPointUrl,
                    string.Format(docDescriptionFormat, description.GroupName.ToUpperInvariant()));
            }

            return options;
        }
    }
}
