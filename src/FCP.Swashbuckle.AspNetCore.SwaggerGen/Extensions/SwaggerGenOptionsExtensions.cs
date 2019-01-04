using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenOptionsExtensions
    {
        public static SwaggerGenOptions AddBearerAuthentication(this SwaggerGenOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            {
                In = "header",
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
            });

            options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            {
                { "Bearer", new string[] { } }
            });

            return options;
        }
    }
}
