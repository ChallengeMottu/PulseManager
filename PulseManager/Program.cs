
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PulseManager.Application.Service.Implementation;
using PulseManager.Application.Service.Interfaces;
using PulseManager.Domain.Entities;
using PulseManager.Infraestruture.Context;
using PulseManager.Infraestruture.Mapping;
using PulseManager.Infraestruture.Repositories.Implementation;
using PulseManager.Infraestruture.Repositories.Interface;


namespace PulseManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pulse Registration Manager",
                    Description = "API de Gerenciamento e Controle de Cadastro de Colaboradores na interface Pulse",
                    Contact = new OpenApiContact()
                    {
                        Name = "Pulse Manager System"
                    }
                });
                swagger.EnableAnnotations();
            });

            builder.Services.AddDbContext<ManagerDbContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddScoped<IMethodsRepository<Usuario>, UsuarioRepository>();
            builder.Services.AddScoped<IMethodsRepository<Login>, LoginRepository>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();

            builder.Services.AddScoped<IUsuarioService, CadastroService>();
            builder.Services.AddScoped<ILoginService, LoginService>();


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}