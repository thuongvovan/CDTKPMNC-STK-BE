using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDTKPMNC_STK_BE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Add_Provinces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOTPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterOtp = table.Column<int>(type: "int", nullable: true),
                    RegisterExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetPasswordOtp = table.Column<int>(type: "int", nullable: true),
                    ResetPasswordExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOTPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Add_Districts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Add_Districts_Add_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Add_Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Add_Wards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProvinceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Add_Wards_Add_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Add_Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Add_Wards_Add_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Add_Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address_WardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountAdmins_Add_Wards_Address_WardId",
                        column: x => x.Address_WardId,
                        principalTable: "Add_Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountEndUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address_WardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountEndUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountEndUsers_Add_Wards_Address_WardId",
                        column: x => x.Address_WardId,
                        principalTable: "Add_Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountPartners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address_WardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPartners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountPartners_Add_Wards_Address_WardId",
                        column: x => x.Address_WardId,
                        principalTable: "Add_Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address_WardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companys_AccountPartners_Id",
                        column: x => x.Id,
                        principalTable: "AccountPartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companys_Add_Wards_Address_WardId",
                        column: x => x.Address_WardId,
                        principalTable: "Add_Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_WardId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenTime = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_AccountPartners_Id",
                        column: x => x.Id,
                        principalTable: "AccountPartners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_Add_Wards_Address_WardId",
                        column: x => x.Address_WardId,
                        principalTable: "Add_Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinRate = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    GameRule = table.Column<int>(type: "int", nullable: false),
                    NumberOfLimit = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Campaigns_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnable = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItems_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductItems_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoucherSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherSeries_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CampaignEndUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsWinner = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignEndUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignEndUsers_AccountEndUsers_EndUserId",
                        column: x => x.EndUserId,
                        principalTable: "AccountEndUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignEndUsers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignEndUsers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CampaignVoucherSeries",
                columns: table => new
                {
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherSeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignVoucherSeries", x => new { x.CampaignId, x.VoucherSeriesId });
                    table.ForeignKey(
                        name: "FK_CampaignVoucherSeries_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignVoucherSeries_VoucherSeries_VoucherSeriesId",
                        column: x => x.VoucherSeriesId,
                        principalTable: "VoucherSeries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherSeriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignEndUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.VoucherCode);
                    table.ForeignKey(
                        name: "FK_Vouchers_AccountEndUsers_EndUserId",
                        column: x => x.EndUserId,
                        principalTable: "AccountEndUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vouchers_CampaignEndUsers_CampaignEndUsersId",
                        column: x => x.CampaignEndUsersId,
                        principalTable: "CampaignEndUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vouchers_CampaignVoucherSeries_CampaignId_VoucherSeriesId",
                        columns: x => new { x.CampaignId, x.VoucherSeriesId },
                        principalTable: "CampaignVoucherSeries",
                        principalColumns: new[] { "CampaignId", "VoucherSeriesId" });
                });

            migrationBuilder.InsertData(
                table: "AccountAdmins",
                columns: new[] { "Id", "CreatedAt", "DateOfBirth", "Department", "Gender", "IsVerified", "Name", "NewPassword", "Password", "Position", "UserName", "VerifiedAt" },
                values: new object[] { new Guid("f6d3b718-8d60-46c6-a5cf-8ac64d25d51a"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, true, null, null, "Bx1NR1brKN05VDVQw7ps4l945lC+3vide5WuPfN1C2Q=", null, "admin", null });

            migrationBuilder.CreateIndex(
                name: "IX_AccountAdmins_Address_WardId",
                table: "AccountAdmins",
                column: "Address_WardId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountEndUsers_Address_WardId",
                table: "AccountEndUsers",
                column: "Address_WardId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountEndUsers_UserName",
                table: "AccountEndUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountPartners_Address_WardId",
                table: "AccountPartners",
                column: "Address_WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Add_Districts_ProvinceId",
                table: "Add_Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Add_Wards_DistrictId",
                table: "Add_Wards",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Add_Wards_ProvinceId",
                table: "Add_Wards",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignEndUsers_CampaignId",
                table: "CampaignEndUsers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignEndUsers_EndUserId",
                table: "CampaignEndUsers",
                column: "EndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignEndUsers_GameId",
                table: "CampaignEndUsers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GameId",
                table: "Campaigns",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_StoreId",
                table: "Campaigns",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignVoucherSeries_VoucherSeriesId",
                table: "CampaignVoucherSeries",
                column: "VoucherSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_Address_WardId",
                table: "Companys",
                column: "Address_WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_BusinessCode",
                table: "Companys",
                column: "BusinessCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notications_AccountId",
                table: "Notications",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ProductCategoryId",
                table: "ProductItems",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_StoreId",
                table: "ProductItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Address_WardId",
                table: "Stores",
                column: "Address_WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CampaignEndUsersId",
                table: "Vouchers",
                column: "CampaignEndUsersId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CampaignId_VoucherSeriesId",
                table: "Vouchers",
                columns: new[] { "CampaignId", "VoucherSeriesId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_EndUserId",
                table: "Vouchers",
                column: "EndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherSeries_StoreId",
                table: "VoucherSeries",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountAdmins");

            migrationBuilder.DropTable(
                name: "Companys");

            migrationBuilder.DropTable(
                name: "Notications");

            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "UserOTPs");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "CampaignEndUsers");

            migrationBuilder.DropTable(
                name: "CampaignVoucherSeries");

            migrationBuilder.DropTable(
                name: "AccountEndUsers");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "VoucherSeries");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "AccountPartners");

            migrationBuilder.DropTable(
                name: "Add_Wards");

            migrationBuilder.DropTable(
                name: "Add_Districts");

            migrationBuilder.DropTable(
                name: "Add_Provinces");
        }
    }
}
