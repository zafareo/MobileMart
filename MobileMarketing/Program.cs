using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace MobileMarketing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .ReadFrom.Configuration(builder.Configuration)
           .CreateLogger();

            try
            {
                builder.Host.UseSerilog();
                IConfiguration configuration = builder.Configuration;
                builder.Services.AddApplication();
                builder.Services.AddInfrastructure(configuration);
                builder.Services.AddWebUIServices();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "Bearer Authentication with JWT Token",
                        Type = SecuritySchemeType.Http
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {{
                        new OpenApiSecurityScheme()
                        {
                           Reference=new OpenApiReference()
                           {
                               Id= JwtBearerDefaults.AuthenticationScheme,
                               Type=ReferenceType.SecurityScheme
                           }
                        },
                        new List<string>()
                    } });
                });


                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                    {
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = true,
                            ValidAudience = configuration["JWT:Audience"],
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JWT:Issuer"],
                            ValidateLifetime = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        };
                    });


                var app = builder.Build();


                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();

            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}