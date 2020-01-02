using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
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

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var newPathDict = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace($"v{{{_versionParameterName}}}",
                                (swaggerDoc.Info.Extensions[SwaggerGenVersioningConstants.SwaggerInfoVersionGroupKey] as OpenApiString).Value),
                    path => path.Value
                );

            swaggerDoc.Paths.Clear();
            foreach(var path in newPathDict)
            {
                swaggerDoc.Paths.Add(path.Key, path.Value);
            }
        }
    }
}
