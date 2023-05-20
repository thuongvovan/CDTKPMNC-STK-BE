using CDTKPMNC_STK_BE.Models;
using System.Runtime.CompilerServices;

namespace CDTKPMNC_STK_BE.BusinessServices.Records
{
    #region Common
    public record AddressRecord(string? WardId, string? Street);
    public record DateRecord(int? Year, int? Month, int? Day);
    public record TimeRecord(int? Hours, int? Minute);
    public record TokenRecord(string? Token);
    #endregion

    #region Account
    public record LoginRecord(string? UserName, string? Password);
    public record ChangePasswordRecord(string? OldPassword, string? NewPassword);
    public record ResetPasswordRecord(string? UserName, string? NewPassword);
    public record VerifyResetPasswordRecord(string? UserName, int? Otp);
    public record AccountRegistrationRecord(string? UserName, string? Password, string? Name, Gender? Gender, DateRecord? BirthDate, AddressRecord? Address);
    public record AccountUpdateRecord(string? Name, Gender? Gender, DateRecord? BirthDate, AddressRecord? Address);
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
    public record StoreRecord(string? Name, string? Description, AddressRecord? Address, TimeRecord? OpenTime, TimeRecord? CloseTime, bool? IsEnable);
    #endregion

    #region Game
    public record GameRecord(string? Name, string? Description, string? Instruction, bool? IsEnable);
    #endregion

    #region Product category
    public record ProductCategoryRecord(string? Name, string? Description, bool? IsEnable);
    #endregion

    #region Product Item
    public record ProductItemRecord(string? Name, string? Description, Guid? ProductCategoryId, float? Price, bool? IsEnable);
    #endregion
}
