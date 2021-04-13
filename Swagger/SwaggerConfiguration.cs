using System;
using System.Collections.Generic;
using MsalDemo.Backend.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MsalDemo.Backend.Swagger
{
    public static class ConfigureSwagger
    {
        private const string Title = "Votemeister 2021";
        private const string SwaggerEndpoint = "/swagger/v1/swagger.json";
        internal const string SecurityDefinitionName = "azure-ad";
        
        private static readonly OpenApiContact Contact = new OpenApiContact()
        {
            Email = "anders.r.olsen@bekk.no",
            Name = "Anders Refsdal Olsen",
            Url = new Uri("https://facebook.com/andersro93"),
        };
        
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var authorizeEndpoint = configuration.GetValue<string>("Swagger:AuthorizeEndpoint");
            var tokenEndpoint = configuration.GetValue<string>("Swagger:TokenEndpoint");

            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Title,
                    Version = "v1",
                    Contact = Contact,
                    Description = "Msal demo api for Frontend for breakfast with Bekk"
                });
                options.AddSecurityDefinition(SecurityDefinitionName, new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(authorizeEndpoint),
                            TokenUrl = new Uri(tokenEndpoint),
                            Scopes =
                            {
                                {
                                    Scopes.FullScopeName(Scopes.CandidatesRead), "Read candidates"
                                },
                                {
                                    Scopes.FullScopeName(Scopes.VotesCast), "Cast new votes"
                                },
                                {
                                    Scopes.FullScopeName(Scopes.VotesRead), "Read all casted votes"
                                },
                                {
                                    Scopes.FullScopeName(Scopes.ResultsRead), "Read the result"
                                }
                            }
                        }
                    }
                });
                
                options.OperationFilter<SwaggerAuthenticationRequirementsOperationFilter>();
            });
            
            return serviceCollection;
        }
        
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(SwaggerEndpoint, Title);
                c.OAuthConfigObject = new OAuthConfigObject()
                {
                    ClientId = configuration.GetValue<string>("Swagger:ClientId"),
                    Scopes = new[]
                    {
                        Scopes.FullScopeName(Scopes.CandidatesRead), 
                        Scopes.FullScopeName(Scopes.VotesCast), 
                        Scopes.FullScopeName(Scopes.VotesRead), 
                        Scopes.FullScopeName(Scopes.ResultsRead)
                    },
                    ScopeSeparator = " ",
                };
            });

            return app;
        }
    }
    
    internal class SwaggerAuthenticationRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, 
                    Id = ConfigureSwagger.SecurityDefinitionName
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}