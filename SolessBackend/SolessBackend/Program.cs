using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using SolessBackend.Data;
using SolessBackend.DataMappers;
using SolessBackend.Interfaces;
using SolessBackend.Repositories;
using System.Text;

namespace SolessBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddTransient<Seeder>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<DataBaseContext>();

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                string key = Environment.GetEnvironmentVariable("JWT_KEY");

                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception("JWT_KEY variable de entorno no está configurada.");
                }

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                DataBaseContext dbcontext = scope.ServiceProvider.GetService<DataBaseContext>();
                dbcontext.Database.EnsureCreated();
            }

            if (args.Length == 1 && args[0].ToLower() == "seeddata")
                SeedData(app);
            void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<Seeder>();
                    service.Seed();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
