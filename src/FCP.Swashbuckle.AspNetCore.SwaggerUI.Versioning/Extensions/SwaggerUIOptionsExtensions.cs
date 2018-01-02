using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Swashbuckle.AspNetCore.SwaggerUI
{
    public static class SwaggerUIOptionsExtensions
    {
        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider)
        {
            return VersioningSwaggerEndpoints(options, provider, "{0} Docs");
        }

        public static SwaggerUIOptions VersioningSwaggerEndpoints(this SwaggerUIOptions options,
            IApiVersionDescriptionProvider provider, string docDescriptionFormat)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (string.IsNullOrWhiteSpace(docDescriptionFormat))
                throw new ArgumentNullException(nameof(docDescriptionFormat));

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    string.Format(docDescriptionFormat, description.GroupName.ToUpperInvariant()));
            }

            return options;
        }
    }
}
