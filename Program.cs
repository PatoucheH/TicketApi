using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Data.Common;
using System.Text;
using TicketApi.Data;
using TicketApi.Models;
using TicketApi.Services;
using TicketApi.Validators;

namespace TicketApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            /// create and options the builder
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            { 
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // add context
            builder.Services.AddDbContext<ContextDatabase>(opt => opt.
                UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // add controller and scope
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserService, UserService>();

            // validate the input with FluentValidator
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();  
            builder.Services.AddValidatorsFromAssemblyContaining<TicketDTOValidator>();

            // Configure Jwt
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var config = builder.Configuration;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = config["JwtSettings:Issuer"],
                        ValidAudience = config["JwtSettings:Audience"], 
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!)
                        )
                    };
                });

            // create and optiosn the app before running it 
            var app = builder.Build();
                // add swagger
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/openapi/{documentName}.json";
                });
                // add extension swagger scalar
                app.MapScalarApiReference();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ContextDatabase>();
                db.Database.Migrate();
                await DbSeeder.Seed(db);
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
