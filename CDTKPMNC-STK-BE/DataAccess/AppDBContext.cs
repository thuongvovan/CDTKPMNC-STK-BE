using CDTKPMNC_STK_BE.Models;
using Microsoft.EntityFrameworkCore;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.Extensions.Configuration;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class AppDBContext : DbContext
    {
        public DbSet<EndUserAccount> EndUserAccounts { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public string ConnectionString { get; set; }
        public ConfigurationManager Configuration { get; set; }
        public AppDBContext(ConfigurationManager configuration)
        {
            Configuration = configuration;
            var connectionString = Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string not found.");
            ConnectionString = connectionString;
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
            // string connectionString = "Data Source=192.168.1.100;Initial Catalog=CD_TKPMNC_tttt;User ID=sa;Password=Zxcv@1234;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(ConnectionString);
            
        }
    }
}
