using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Linq;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public class SetVersionInPaths : IDocumentFilter
    {
        private readonly string _versionParameterName;

        public SetVersionInPaths(string versionParamName)
        {
            if (string.IsNullOrWhiteSpace(versionParamName))
                throw new ArgumentNullException(nameof(versionParamName));

            _versionParameterName = versionParamName;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace($"v{{{_versionParameterName}}}",
                                swaggerDoc.Info.Extensions[SwaggerGenVersioningConstants.SwaggerInfoVersionGroupKey].ToString()),
                    path => path.Value
                );
        }
    }
}
