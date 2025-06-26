using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using TicketApi.Data;
using TicketApi.Models;
using TicketApi.Services;
using TicketApi.Validators;

namespace TicketApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /// create and options the builder
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // add context
            builder.Services.AddDbContext<ContextDatabase>(opt => opt.
                UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
            // add controller and scope
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserService, UserService>();
            // validate the input with FluentValidator
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();  
            builder.Services.AddValidatorsFromAssemblyContaining<TicketDTOValidator>();  


            // create and optiosn the app before running it 
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                // add swagger
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/openapi/{documentName}.json";
                });
                // add extension swagger scalar
                app.MapScalarApiReference();
            }
            app.MapControllers();

            app.Run();
        }
    }
}
