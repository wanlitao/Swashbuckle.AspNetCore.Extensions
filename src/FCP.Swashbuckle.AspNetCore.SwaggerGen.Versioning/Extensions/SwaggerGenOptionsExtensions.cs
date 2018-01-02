using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions VersioningSwaggerDoc(this SwaggerGenOptions options,
            IApiVersionDescriptionProvider provider)
        {
            return VersioningSwaggerDoc(options, provider, "API {0}");
        }

        public static SwaggerGenOptions VersioningSwaggerDoc(this SwaggerGenOptions options,
            IApiVersionDescriptionProvider provider, string docTitleFormat)
        {
            return VersioningSwaggerDoc(options, provider, docTitleFormat, "api-version");
        }

        public static SwaggerGenOptions VersioningSwaggerDoc(this SwaggerGenOptions options,
            IApiVersionDescriptionProvider provider, string docTitleFormat, string versionParamName)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (string.IsNullOrWhiteSpace(docTitleFormat))
                throw new ArgumentNullException(nameof(docTitleFormat));

            if (string.IsNullOrWhiteSpace(versionParamName))
                throw new ArgumentNullException(nameof(versionParamName));

            foreach (var description in provider.ApiVersionDescriptions)
            {
                var info = new Info()
                {
                    Title = string.Format(docTitleFormat, description.ApiVersion),
                    Version = description.ApiVersion.ToString()
                };
                info.Extensions.Add(SwaggerGenVersioningConstants.SwaggerInfoVersionGroupKey, description.GroupName);

                options.SwaggerDoc(description.GroupName, info);
            }

            options.OperationFilter<RemoveVersionParameters>(versionParamName);
            options.DocumentFilter<SetVersionInPaths>(versionParamName);

            return options;
        }
    }
}
