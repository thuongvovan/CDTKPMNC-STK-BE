using CDTKPMNC_STK_BE.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDTKPMNC_STK_BE.BusinessServices.Records
{
    #region Common
    public record AddressRecord(string? WardId, string? Street);
    public record DateRecord(int? Year, int? Month, int? Day);
    public record TimeRecord(int? Hour, int? Minute);
    public record TokenRecord(string? Token);
    #endregion

    #region Account
    public record LoginRecord(string? UserName, string? Password);
    public record ChangePasswordRecord(string? OldPassword, string? NewPassword);
    public record ResetPasswordRecord(string? UserName, string? NewPassword);
    public record VerifyResetPasswordRecord(string? UserName, int? Otp);
    public record AccountRegistrationRecord(string? UserName, string? Password, string? Name, Gender? Gender, DateRecord? DateOfBirth, AddressRecord? Address);
    public record AccountUpdateRecord(string? Name, Gender? Gender, DateRecord? DateOfBirth, AddressRecord? Address);
    #endregion

    #region Admin
    public record AdminUpdateRecord(AccountUpdateRecord? AccountUpdate, string? Position, string? Department);
    #endregion

    #region EndUser
    #endregion

    #region Partner
    public record PartnerRegistrationRecord(AccountRegistrationRecord? Account, PartnerType? PartnerType, CompanyRecord? Company);
    public record PartnerUpdateRecord(AccountUpdateRecord? AccountUpdate, PartnerType? PartnerType, CompanyRecord? Company);
    public record CompanyRecord(string? Name, string? BusinessCode, AddressRecord? Address);
    #endregion

    #region Store
    public record StoreRecord(string? Name, string? Description, AddressRecord? Address, TimeRecord? OpenTime, TimeRecord? CloseTime, bool? IsEnable, string? BannerUrl);
    /// <summary>
    /// Thông tin trả về cho enduser
    /// </summary>
    public class StoreReturn_E
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string? BannerUrl { get; set; }
        public CampaignReturn? Campaign { get; set; } = null!; // check lại xem chỉnh model Campaign không
    }
    #endregion

    #region Game
    public record GameRecord(string? Name, string? Description, string? Instruction, string? ImageUrl, bool? IsEnable);
    #endregion

    #region Campaign
    public record CampaignCreateRecord(CampaignInfoRecord? CampaignInfo, CampaignVoucherSeriesRecord[]? CampaignVoucherSeriesList);
    public record CampaignInfoRecord(string? Name, string? Description, DateRecord? StartDate, DateRecord? EndDate, Guid? GameId, int? WinRate, GameRule? GameRule, int? NumberOfLimit, bool? IsEnable);
    public record CampaignVoucherSeriesRecord(Guid? VoucherSeriesId, int? Quantity, DateRecord? ExpiresOn);
    public class CampaignReturn
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public Guid GameId { get; set; }
        public string GameName { get; set; } = null!;
        public int WinRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEnable { get; set; }
        public CampaignVoucherSeriesReturn[] CampaignVoucherList { get; set; } = null!;
        [JsonConverter(typeof(StringEnumConverter))]
        public CampaignStatus Status { get; set; }
        public GameRule GameRule { get; set; }
        public int? NumberOfLimit { get; set; }
    }

    #endregion

    #region Voucher
    public record VoucherSeriesRecord(string? Name, string? Description);
    public record VoucherSeriesDeleteRecord(Guid VoucherSeriesId);
    public record VoucherShareRecord(Guid? VoucherCode, string? DestinationUser);

    public class VoucherSeriesReturn
    {
        public VoucherSeriesReturn() { }
        public VoucherSeriesReturn(Guid id, string name, string description, Guid storeId)
        {
            Id = id;
            Name = name;
            Description = description;
            StoreId = storeId;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;    
        public string Description { get; set; } = null!;
        public Guid StoreId { get; set; }
    }

    public class CampaignVoucherSeriesReturn
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public DateOnly ExpiresOn { get; set; }
    }

    public class VoucherReturn
    {
        public Guid VoucherCode { get; set; }
        public string VoucherName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public Guid CampaignId { get; set; }
        public string CampaignName { get; set; } = null!;
        public Guid EndUserId { get; set; }
        public string EndUserName { get; set; } = null!;
        public DateOnly ExpiresOn { get; set; }
        public bool IsUsed { get; set; }
    }

    #endregion

    #region Product category
    public record ProductCategoryRecord(string? Name, string? Description, bool? IsEnable);
    #endregion

    #region Product Item
    public record ProductItemRecord(string? Name, string? Description, Guid? ProductCategoryId, float? Price, bool? IsEnable, string? ImageUrl);
    #endregion

    #region Notication
    public class NoticaionsReturn
    {
        public bool HaveUnread { get; set; }
        public int NumberUnread { get; set; }
        public Notication[] Notications { get; set; } =  Array.Empty<Notication>();
    }

    #endregion

    #region Cache
    public record PartnerCount(int All, int Verified);
    public record StoreCount(int All, int NeedApproved, int Approved, int Rejected);
    public record EndUserCount(int All, int Verified);

    #endregion

}