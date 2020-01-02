using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public class RemoveVersionParameters : IOperationFilter
    {
        private readonly string _versionParameterName;

        public RemoveVersionParameters(string versionParamName)
        {
            if (string.IsNullOrWhiteSpace(versionParamName))
                throw new ArgumentNullException(nameof(versionParamName));

            _versionParameterName = versionParamName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Remove version parameter from all Operations
            var versionParameter = operation.Parameters.SingleOrDefault(p => string.Compare(p.Name, _versionParameterName, true) == 0);

            if (versionParameter != null)
            {
                operation.Parameters.Remove(versionParameter);

                operation.OperationId = context.ApiDescription.FriendlyId(_versionParameterName);
            }                
        }
    }
}
