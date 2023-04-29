using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Utilities;
using CDTKPMNC_STK_BE.Utilities.Email;
using Microsoft.Extensions.DependencyInjection;

namespace CDTKPMNC_STK_BE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* 
            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json");
            var configuration = configBuilder.Create();
            string? connectionString = configuration.GetConnectionString("Default");
            */

            var builder = WebApplication.CreateBuilder(args);

            var secretKey = builder.Configuration.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Secret Key not found.");

            // Add services to the container.
            builder.Services.AddControllers();

            var dbContext = new AppDBContext(builder.Configuration);
            builder.Services.AddSingleton(dbContext);
            // builder.Services.AddSingleton(builder.Configuration);
            builder.Services.AddSingleton(new  EmailService(builder.Configuration));
            builder.Services.AddSingleton(new Cryptography(builder.Configuration));


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            var jwtAuthenSchemaBuiler = new JwtAuthenSchema(builder.Configuration);
            builder.Services.AddAuthentication()
                .AddJwtBearer("EndUser", jwtAuthenSchemaBuiler.Create(TokenType.Access, UserType.EndUser, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("EndUserNoLifetime",
                    new JwtAuthenSchema(builder.Configuration)
                        .Create(TokenType.Access, UserType.EndUser, false));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Admin", jwtAuthenSchemaBuiler.Create(TokenType.Access, UserType.Admin, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("AdminNoLifetime", jwtAuthenSchemaBuiler.Create(TokenType.Access, UserType.Admin, false));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Parner", jwtAuthenSchemaBuiler.Create(TokenType.Access, UserType.Parner, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("ParnerNoLifetime", jwtAuthenSchemaBuiler.Create(TokenType.Access, UserType.Parner, false));

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseCors(configurePolicy =>
            {
                configurePolicy.AllowAnyOrigin();
                configurePolicy.AllowAnyMethod();
                configurePolicy.AllowAnyHeader();
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}