using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.DatabaseContext;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using CDTKPMNC_STK_BE.Utilities.Email;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                .UseLazyLoadingProxies();
            });

            // builder.Services.AddSingleton(builder.Configuration);
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            //var dbContext = new AppDbContext(builder.Configuration);
            //builder.Services.AddSingleton(dbContext);

            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtAuthen = new JwtAuthen(builder.Configuration);
            builder.Services.AddSingleton(jwtAuthen);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });


            builder.Services.AddAuthentication()
                .AddJwtBearer("EndUser", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, UserType.EndUser));
            builder.Services.AddAuthentication()
               .AddJwtBearer("EndUserNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, UserType.EndUser));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Admin", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, UserType.Admin));
            builder.Services.AddAuthentication()
               .AddJwtBearer("AdminNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, UserType.Admin));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Partner", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, UserType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("PartnerNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, UserType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("Admin&Partner", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, UserType.Admin, UserType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("Account", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, UserType.Admin, UserType.Partner, UserType.EndUser));

            var app = builder.Build();

            if (true || app.Environment.IsDevelopment())
            {
                using var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
                // Delete the database if it exists
                dbContext.Database.EnsureDeleted();
                Console.WriteLine("Delete the database if it exists");
                // Create the database and its tables
                dbContext.Database.EnsureCreated();
                Console.WriteLine("Create the database and its tables");

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

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