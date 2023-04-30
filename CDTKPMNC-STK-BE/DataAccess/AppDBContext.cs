using CDTKPMNC_STK_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class AppDBContext : DbContext
    {
        public DbSet<EndUserAccount> EndUserAccounts { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        //private readonly IConfiguration _configuration;
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) // IConfiguration configuration
        {
            //_configuration = configuration;
            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env != null && (env == "Development" || env == "Testing"))
            {
                // Database.EnsureDeleted();
                Database.EnsureCreated();
                Console.WriteLine("Khởi tạo DB context");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EndUserAccount>()
                .HasIndex(ua => ua.Account)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //var connectionString = _configuration.GetConnectionString("Default");
            //optionsBuilder.UseSqlServer(connectionString)
            //    .UseLazyLoadingProxies();

        }
    }
}