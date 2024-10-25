
using SolessBackend.Data;
using SolessBackend.Interfaces;
using SolessBackend.Repositories;

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<DataBaseContext>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
