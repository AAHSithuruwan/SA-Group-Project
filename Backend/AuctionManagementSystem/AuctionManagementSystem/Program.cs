using AuctionManagementSystem.Data;
using AuctionManagementSystem.JwtAuthentication;
using AuctionManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuctionManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173") // Allow the frontend's origin
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

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

            builder.Services.AddSingleton<JwtAuthenticationService>();

            builder.Services.AddSwaggerGen();

            //Jwt configuration starts here
            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtSecretKey = builder.Configuration.GetSection("Jwt:SecretKey").Get<string>();

            if(jwtIssuer == null)
            {
                throw new Exception("JWT Issuer is null");
            }

            if (jwtSecretKey == null)
            {
                throw new Exception("JWT SecretKey is null");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtIssuer,
                     ValidAudience = jwtIssuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                 };
             });
            //Jwt configuration ends here

            var app = builder.Build();

            // Serve static files from the specified directory
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "CategoryImages")),
                RequestPath = "/Images/CategoryImages"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "ProductImages")),
                RequestPath = "/Images/ProductImages"
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            // Use CORS middleware
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


