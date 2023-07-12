using MessagingAppApi.Hubs;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MessagingAppApi.Shared.Swagger
{
    public static class Extensions
    {
        private const string ProjectTitleSwaggerApi = "MessagingAppApi";
        private const string SwaggerVersion = "v1";
        private const string SecurityName = "Bearer";



        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ProjectTitleSwaggerApi, new OpenApiInfo { Title = ProjectTitleSwaggerApi, Version = SwaggerVersion });

                c.AddSecurityDefinition(SecurityName, openApiSecurityScheme);
                c.AddSecurityRequirement(openApiSecurityRequirement);
            });

            return services;
        }
        public static IApplicationBuilder UseSwaggerOptions(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ProjectTitleSwaggerApi}/swagger.json", ProjectTitleSwaggerApi);
            });
            return app;
        }

        private static OpenApiSecurityScheme openApiSecurityScheme
            => new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            };
        private static OpenApiSecurityRequirement openApiSecurityRequirement
            => new OpenApiSecurityRequirement()
            {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = SecurityName
                }
            },
                new string[] { }
            }
            };
    }
}
