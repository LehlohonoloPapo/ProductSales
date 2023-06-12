using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductSales.Controllers;
using ProductSales.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using ProductSales.Middleware;
using Serilog;
using Serilog.Sinks.ElmahIo;


namespace ProductSales
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
             Log.Logger = new LoggerConfiguration()
                 .WriteTo.ElmahIo(
                     new ElmahIoSinkOptions(
                         "b4dfcee77f344bc790d8d96f3abe0804",
                         new Guid("18d20dc8-1dba-4d88-a26b-2e3a1e72f742")
                     )).CreateLogger();
            
            
            // Add services to the container.
        
            builder.Services.AddDbContext<ProductCoreContext>();

            // Add JWT authentication
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Secrets:issuer"],
                        ValidAudience = configuration["Secrets:audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secrets:secretKey"]))
                    };
                });
           
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Add the Swagger document generation configuration here

                // Add a security definition for bearer token authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT"
                });

                // Require the bearer token to be passed in the request headers
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
            try
            {
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                ;

                app.UseHttpsRedirection();

                app.UseAuthentication();

                app.UseAuthorization();

                /*app.Use(async (context, next) =>
                {
                    try
                    {
                        Debug.WriteLine("debug");
                       await next(context);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        context.Response.StatusCode = 500;
                    }
                });*/

                app.UseMiddleware<GlobalExceptionHandling>();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }   
        }
    }
}