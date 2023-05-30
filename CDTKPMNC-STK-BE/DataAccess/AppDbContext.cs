using CDTKPMNC_STK_BE.DataAccess.DbConfigurations;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEndUser> AccountEndUsers { get; set; }
        public DbSet<AccountOtp> UserOTPs { get; set; }
        public DbSet<AccountToken> UserTokens { get; set; }
        public DbSet<AccountAdmin> AccountAdmins { get; set; }
        public DbSet<AccountPartner> AccountPartners { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignVoucherSeries> CampaignVoucherSeries { get; set; }
        public DbSet<CampaignEndUsers> CampaignEndUsers { get; set; }
        public DbSet<VoucherSeries> VoucherSeries { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<AddressProvince> Provinces { get; set; }
        public DbSet<AddressDistrict> Districts { get; set; }
        public DbSet<AddressWard> Wards { get; set; }
        public DbSet<Notication> Notications { get; set; }

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
                        UserName = adminUserName.ToLower(),
                        Password = adminPassword.ToHashSHA256(),
                        IsVerified = true,
                    });
            modelBuilder.ApplyConfiguration(new ConfigCampaign());
            modelBuilder.ApplyConfiguration(new ConfigStore());
            modelBuilder.ApplyConfiguration(new ConfigVoucherSeries());
            modelBuilder.ApplyConfiguration(new ConfigVoucherSeriesCampaigns());
            //modelBuilder.ApplyConfiguration(new SeedProvince());
            //modelBuilder.ApplyConfiguration(new SeedDistrict());
            //modelBuilder.ApplyConfiguration(new SeedWard());


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}