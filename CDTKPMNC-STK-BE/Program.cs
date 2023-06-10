using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using CDTKPMNC_STK_BE.BusinessServices;
using Microsoft.Extensions.FileProviders;
using Quartz;
using Quartz.Impl;

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
                options.UseSqlServer(builder.Configuration.GetConnectionString("Testing")) // Testing   Default   Dev-new
                        .UseLazyLoadingProxies();
            });

            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = builder.Configuration.GetValue<string>("RedisConnection");
                options.InstanceName = "CDTKPMNC_Redis_";
            });

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            //var dbContext = new AppDbContext(builder.Configuration);
            //builder.Services.AddSingleton(dbContext);

            //builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtAuthen = new JwtAuthen(builder.Configuration);
            builder.Services.AddSingleton(jwtAuthen);

            // Business service dependency injection
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<PartnerService>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<EndUserService>();
            builder.Services.AddScoped<AccountService<Account>>();
            builder.Services.AddScoped<OtpService>();
            builder.Services.AddScoped<CompanyService>();
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<StoreService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<ProductCategoryService>();
            builder.Services.AddScoped<ProductItemService>();
            builder.Services.AddScoped<CampaignService>();
            builder.Services.AddScoped<VoucherService>();
            builder.Services.AddScoped<NoticationService>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            
            builder.Services.AddAuthorization();
            builder.Services.AddRouting();

            builder.Services.AddAuthentication()
                .AddJwtBearer("EndUser", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, AccountType.EndUser));
            builder.Services.AddAuthentication()
               .AddJwtBearer("EndUserNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, AccountType.EndUser));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Admin", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, AccountType.Admin));
            builder.Services.AddAuthentication()
               .AddJwtBearer("AdminNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, AccountType.Admin));
            builder.Services.AddAuthentication()
                .AddJwtBearer("Partner", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, AccountType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("PartnerNoLifetime", jwtAuthen.CreateAuthenSchema(TokenType.Access, false, AccountType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("Admin&Partner", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, AccountType.Admin, AccountType.Partner));
            builder.Services.AddAuthentication()
               .AddJwtBearer("Account", jwtAuthen.CreateAuthenSchema(TokenType.Access, true, AccountType.Admin, AccountType.Partner, AccountType.EndUser));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddQuartz(q =>
            {
                // Just use the name of your job that you created in the Jobs folder.
                var jobKey = new JobKey("CleanTemporaryFiles");
                q.AddJob<CleanTemporaryFiles>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("CleanTemporaryFiles-trigger")
                    .WithSchedule(CronScheduleBuilder.CronSchedule("0 20 * ? * * *")) //0 * * ? * * *
                    );
            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            var app = builder.Build();     

            if (true || app.Environment.IsDevelopment())
            {
                using var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
                // Delete the database if it exists
                // dbContext.Database.EnsureDeleted();
                // Console.WriteLine("Delete the database if it exists");
                // Create the database and its tables

                // dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
                // Console.WriteLine("Create the database and its tables");

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseRouting();
            app.UseCors(configurePolicy =>
            {
                configurePolicy.AllowAnyOrigin();
                configurePolicy.AllowAnyMethod();
                configurePolicy.AllowAnyHeader();
            });
            app.UseStaticFiles();
            string uploadDirectory = Environment.GetEnvironmentVariable("UPLOAD_DIRECTORY")!;
            string uploadRequestPath = Environment.GetEnvironmentVariable("UPLOAD_REQUEST_PATH")!;

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadDirectory),
                RequestPath = uploadRequestPath
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}