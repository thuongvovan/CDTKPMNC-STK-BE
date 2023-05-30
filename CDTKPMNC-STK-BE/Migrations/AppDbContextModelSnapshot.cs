﻿// <auto-generated />
using System;
using CDTKPMNC_STK_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CDTKPMNC_STK_BE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountOtp", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("RegisterExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RegisterOtp")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ResetPasswordExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ResetPasswordOtp")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserOTPs");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressDistrict", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Add_Districts");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressProvince", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEN")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Add_Provinces");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressWard", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DistrictId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Add_Wards");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Campaign", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GameRule")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfLimit")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("WinRate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("StoreId");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignEndUsers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampaignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EndUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("IsWinner")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PlatAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.HasIndex("EndUserId");

                    b.HasIndex("GameId");

                    b.ToTable("CampaignEndUsers");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignVoucherSeries", b =>
                {
                    b.Property<Guid>("CampaignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VoucherSeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("date");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CampaignId", "VoucherSeriesId");

                    b.HasIndex("VoucherSeriesId");

                    b.ToTable("CampaignVoucherSeries");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BusinessCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessCode")
                        .IsUnique();

                    b.ToTable("Companys");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Notication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Notications");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.ProductCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.ProductItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<Guid>("ProductCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("ProductItems");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ApprovedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("BannerUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("CloseTime")
                        .HasColumnType("time(0)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("OpenTime")
                        .HasColumnType("time(0)");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Voucher", b =>
                {
                    b.Property<Guid>("VoucherCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampaignEndUsersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampaignId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EndUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<Guid>("VoucherSeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("VoucherCode");

                    b.HasIndex("CampaignEndUsersId")
                        .IsUnique();

                    b.HasIndex("EndUserId");

                    b.HasIndex("CampaignId", "VoucherSeriesId");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.VoucherSeries", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("VoucherSeries");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountAdmin", b =>
                {
                    b.HasBaseType("CDTKPMNC_STK_BE.Models.Account");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AccountAdmins");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f6d3b718-8d60-46c6-a5cf-8ac64d25d51a"),
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = 0,
                            IsVerified = true,
                            Password = "Bx1NR1brKN05VDVQw7ps4l945lC+3vide5WuPfN1C2Q=",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountEndUser", b =>
                {
                    b.HasBaseType("CDTKPMNC_STK_BE.Models.Account");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("AccountEndUsers");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountPartner", b =>
                {
                    b.HasBaseType("CDTKPMNC_STK_BE.Models.Account");

                    b.Property<int>("PartnerType")
                        .HasColumnType("int");

                    b.ToTable("AccountPartners");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountOtp", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Account", "Account")
                        .WithOne("Otp")
                        .HasForeignKey("CDTKPMNC_STK_BE.Models.AccountOtp", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountToken", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Account", "Account")
                        .WithOne("AccountToken")
                        .HasForeignKey("CDTKPMNC_STK_BE.Models.AccountToken", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressDistrict", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.AddressProvince", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Province");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressWard", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.AddressDistrict", "District")
                        .WithMany("Wards")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("CDTKPMNC_STK_BE.Models.AddressProvince", "Province")
                        .WithMany("Wards")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("District");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Campaign", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Game", "Game")
                        .WithMany("Campaigns")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.Store", "Store")
                        .WithMany("Campaigns")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignEndUsers", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Campaign", "Campaign")
                        .WithMany("CampaignEndUsersList")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.AccountEndUser", "EndUser")
                        .WithMany("CampaignEndUsersList")
                        .HasForeignKey("EndUserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Campaign");

                    b.Navigation("EndUser");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignVoucherSeries", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Campaign", "Campaign")
                        .WithMany("CampaignVoucherSeriesList")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.VoucherSeries", "VoucherSeries")
                        .WithMany("CampaignVoucherSeriesList")
                        .HasForeignKey("VoucherSeriesId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Campaign");

                    b.Navigation("VoucherSeries");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Company", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.AccountPartner", "AccountPartner")
                        .WithOne("Company")
                        .HasForeignKey("CDTKPMNC_STK_BE.Models.Company", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.OwnsOne("CDTKPMNC_STK_BE.Models.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CompanyId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("WardId")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("CompanyId");

                            b1.HasIndex("WardId");

                            b1.ToTable("Companys");

                            b1.WithOwner()
                                .HasForeignKey("CompanyId");

                            b1.HasOne("CDTKPMNC_STK_BE.Models.AddressWard", "Ward")
                                .WithMany()
                                .HasForeignKey("WardId")
                                .OnDelete(DeleteBehavior.ClientCascade);

                            b1.Navigation("Ward");
                        });

                    b.Navigation("AccountPartner");

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Notication", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Account", "Account")
                        .WithMany("Notications")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.ProductItem", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.ProductCategory", "ProductCategory")
                        .WithMany("ProductItems")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.Store", "Store")
                        .WithMany("ProductItems")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ProductCategory");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Store", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.AccountPartner", "AccountPartner")
                        .WithOne("Store")
                        .HasForeignKey("CDTKPMNC_STK_BE.Models.Store", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.OwnsOne("CDTKPMNC_STK_BE.Models.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("StoreId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("WardId")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("StoreId");

                            b1.HasIndex("WardId");

                            b1.ToTable("Stores");

                            b1.WithOwner()
                                .HasForeignKey("StoreId");

                            b1.HasOne("CDTKPMNC_STK_BE.Models.AddressWard", "Ward")
                                .WithMany()
                                .HasForeignKey("WardId")
                                .OnDelete(DeleteBehavior.ClientCascade);

                            b1.Navigation("Ward");
                        });

                    b.Navigation("AccountPartner");

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Voucher", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.CampaignEndUsers", "CampaignEndUsers")
                        .WithOne("Voucher")
                        .HasForeignKey("CDTKPMNC_STK_BE.Models.Voucher", "CampaignEndUsersId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.AccountEndUser", "EndUser")
                        .WithMany("Vouchers")
                        .HasForeignKey("EndUserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("CDTKPMNC_STK_BE.Models.CampaignVoucherSeries", "CampaignVoucherSeries")
                        .WithMany("Vouchers")
                        .HasForeignKey("CampaignId", "VoucherSeriesId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("CampaignEndUsers");

                    b.Navigation("CampaignVoucherSeries");

                    b.Navigation("EndUser");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.VoucherSeries", b =>
                {
                    b.HasOne("CDTKPMNC_STK_BE.Models.Store", "Store")
                        .WithMany("VoucherSeries")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountAdmin", b =>
                {
                    b.OwnsOne("CDTKPMNC_STK_BE.Models.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("AccountAdminId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("WardId")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("AccountAdminId");

                            b1.HasIndex("WardId");

                            b1.ToTable("AccountAdmins");

                            b1.WithOwner()
                                .HasForeignKey("AccountAdminId");

                            b1.HasOne("CDTKPMNC_STK_BE.Models.AddressWard", "Ward")
                                .WithMany()
                                .HasForeignKey("WardId")
                                .OnDelete(DeleteBehavior.ClientCascade);

                            b1.Navigation("Ward");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountEndUser", b =>
                {
                    b.OwnsOne("CDTKPMNC_STK_BE.Models.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("AccountEndUserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("WardId")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("AccountEndUserId");

                            b1.HasIndex("WardId");

                            b1.ToTable("AccountEndUsers");

                            b1.WithOwner()
                                .HasForeignKey("AccountEndUserId");

                            b1.HasOne("CDTKPMNC_STK_BE.Models.AddressWard", "Ward")
                                .WithMany()
                                .HasForeignKey("WardId")
                                .OnDelete(DeleteBehavior.ClientCascade);

                            b1.Navigation("Ward");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountPartner", b =>
                {
                    b.OwnsOne("CDTKPMNC_STK_BE.Models.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("AccountPartnerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("WardId")
                                .HasColumnType("nvarchar(450)");

                            b1.HasKey("AccountPartnerId");

                            b1.HasIndex("WardId");

                            b1.ToTable("AccountPartners");

                            b1.WithOwner()
                                .HasForeignKey("AccountPartnerId");

                            b1.HasOne("CDTKPMNC_STK_BE.Models.AddressWard", "Ward")
                                .WithMany()
                                .HasForeignKey("WardId")
                                .OnDelete(DeleteBehavior.ClientCascade);

                            b1.Navigation("Ward");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Account", b =>
                {
                    b.Navigation("AccountToken");

                    b.Navigation("Notications");

                    b.Navigation("Otp");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressDistrict", b =>
                {
                    b.Navigation("Wards");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AddressProvince", b =>
                {
                    b.Navigation("Districts");

                    b.Navigation("Wards");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Campaign", b =>
                {
                    b.Navigation("CampaignEndUsersList");

                    b.Navigation("CampaignVoucherSeriesList");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignEndUsers", b =>
                {
                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.CampaignVoucherSeries", b =>
                {
                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Game", b =>
                {
                    b.Navigation("Campaigns");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.ProductCategory", b =>
                {
                    b.Navigation("ProductItems");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.Store", b =>
                {
                    b.Navigation("Campaigns");

                    b.Navigation("ProductItems");

                    b.Navigation("VoucherSeries");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.VoucherSeries", b =>
                {
                    b.Navigation("CampaignVoucherSeriesList");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountEndUser", b =>
                {
                    b.Navigation("CampaignEndUsersList");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("CDTKPMNC_STK_BE.Models.AccountPartner", b =>
                {
                    b.Navigation("Company");

                    b.Navigation("Store");
                });
#pragma warning restore 612, 618
        }
    }
}
