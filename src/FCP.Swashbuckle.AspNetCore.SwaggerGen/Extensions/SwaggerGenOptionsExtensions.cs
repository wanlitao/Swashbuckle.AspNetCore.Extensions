using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

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

            return options;
        }
    }
}
