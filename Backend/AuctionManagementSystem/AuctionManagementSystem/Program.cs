using AuctionManagementSystem.Data;
using AuctionManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //This below section is for the connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<UserService>(); // Register the UserService
            builder.Services.AddScoped<SellerService>(); // Register the SellerService
            builder.Services.AddScoped<AuctionService>(); // Register the AuctionService
            builder.Services.AddScoped<BidService>(); // Register the BidService
            builder.Services.AddScoped<CategoryService>(); // Register the CategoryService

            builder.Services.AddSwaggerGen();

            //Session Management
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}


