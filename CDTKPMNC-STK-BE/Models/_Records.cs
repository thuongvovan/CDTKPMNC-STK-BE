namespace CDTKPMNC_STK_BE.Models
{
    public record EndUserRegistrationInfo(string UserName, string Password, string Name, Gender Gender, DateInfo BirthDate, AddressInfo Address);
    public record EndUserInfo(string Name, Gender Gender, DateInfo BirthDate, AddressInfo Address);
    public record PartnerRegistrationInfo(string UserName, string Password, string Name, Gender Gender, DateInfo BirthDate, AddressInfo Address, PartnerType Type);
    public record PartnerInfo(string Name, Gender Gender, DateInfo BirthDate, AddressInfo Address, PartnerType Type);
    public record AdminInfo(string Name, Gender Gender, DateInfo BirthDate, string? Position, string? Department);
    public record AddressInfo(string WardId, string Street);
    public record CompanyInfo(string Name, string BusinessCode, AddressInfo Address);
    public record StoreInfo(string Name, string Description, AddressInfo Address, TimeInfo OpenTime, TimeInfo CloseTime, bool IsEnable);
    public record LoginInfo(string UserName, string Password);
    public record ChangePasswordInfo(string OldPassword, string NewPassword);
    public record ResetPasswordInfo(string UserName, string NewPassword);
    public record VerifyResetPasswordInfo(string UserName, int Otp);
    public record DateInfo(int Year, int Month, int Day);
    public record TimeInfo(int Hours, int Minute);
    public record RefreshToken(string Token);
    public record ProductCategoryInfo(string Name, string Description, bool IsEnable);
    public record GameInfo(string Name, string Description, string Instruction, bool IsEnable);

}
