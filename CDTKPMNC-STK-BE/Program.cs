using CDTKPMNC_STK_BE.DataAccess;
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

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
                .UseLazyLoadingProxies();
            });

            // builder.Services.AddSingleton(builder.Configuration);
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            //var dbContext = new AppDBContext(builder.Configuration);
            //builder.Services.AddSingleton(dbContext);

            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtAuthen = new JwtAuthen(builder.Configuration);
            builder.Services.AddSingleton(jwtAuthen);


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            builder.Services.AddAuthentication()
                .AddJwtBearer("EndUser", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.EndUser, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("EndUserNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.EndUser, false));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Admin", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.Admin, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("AdminNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.Admin, false));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Parner", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.Parner, true));
            builder.Services.AddAuthentication()
               .AddJwtBearer("ParnerNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, UserType.Parner, false));

            var app = builder.Build();

            if (true || app.Environment.IsDevelopment())
            {
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