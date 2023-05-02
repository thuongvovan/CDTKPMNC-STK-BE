using CDTKPMNC_STK_BE.DatabaseContext.P02_FluentApi;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEndUser> AccountEndUsers { get; set; }
        public DbSet<OtpAccount> UserOTPs { get; set; }
        public DbSet<TokenAccount> UserTokens { get; set; }
        public DbSet<AccountAdmin> AdminAccounts { get; set; }
        public DbSet<AccountPartner> AccountPartners { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<VoucherSeries> VoucherSeries { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<AddressProvince> Provinces { get; set; }
        public DbSet<AddressDistrict> Districts { get; set; }
        public DbSet<AddressWard> Wards { get; set; }


        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ConfigAccount());
            modelBuilder.ApplyConfiguration(new ConfigAccountEndUser());
            var adminUserName = _configuration.GetSection("AdminAccount").GetValue<string>("UserName");
            var adminPassword = _configuration.GetSection("AdminAccount").GetValue<string>("Password");
            modelBuilder.Entity<AccountAdmin>()
                .HasData(
                    new AccountAdmin
                    {
                        Id = Guid.NewGuid(),
                        UserName = adminUserName,
                        Password = adminPassword.ToHashSHA256(),
                        IsVerified = true,
                    });
            modelBuilder.ApplyConfiguration(new ConfigCampaign());
            modelBuilder.ApplyConfiguration(new ConfigStore());
            modelBuilder.ApplyConfiguration(new ConfigVoucherSeries());
            modelBuilder.ApplyConfiguration(new SeedProvince());
            modelBuilder.ApplyConfiguration(new SeedDistrict());
            modelBuilder.ApplyConfiguration(new SeedWard());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            /*
            var connectionString = _configuration.GetConnectionString("Default");
            optionsBuilder.UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
            */
        }
    }
}