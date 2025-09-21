using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PulseManager.Infraestruture.Context;

namespace PulseManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pulse Manager API",
                    Version = "v1",
                    Description = "API para gerenciamento de pátios, zonas e gateways - Mottu Challenge",
                    Contact = new OpenApiContact()
                    {
                        Name = "Pulse Manager System",
                        Email = "contato@pulsemanager.com"
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                
                // Habilita annotations do Swagger
                c.EnableAnnotations();
                
                // Adiciona descrição para os status codes
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Name = "X-API-Key",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "API Key necessária para acessar os endpoints"
                });
            });

            // Configuração do Banco de Dados Oracle
            builder.Services.AddDbContext<ManagerDbContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pulse Manager API v1");
                    c.RoutePrefix = string.Empty; // Define Swagger na raiz
                    c.DocumentTitle = "Pulse Manager API Documentation";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();
        }
    }
}