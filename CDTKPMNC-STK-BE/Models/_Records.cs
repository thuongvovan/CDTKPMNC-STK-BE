namespace CDTKPMNC_STK_BE.Models
{
    public record EndUserRegistrationInfo(string UserName, string Password, string Name, Gender Gender, Date BirthDate, AddressRegistrationInfo Address);
    public record PartnerRegistrationInfo(string UserName, string Password, string Name, Gender Gender, Date BirthDate, AddressRegistrationInfo Address, PartnerType Type);
    public record AddressRegistrationInfo(string WardId, string Street);
    public record CompanyRegistrationInfo(string Name, string BusinessCode, AddressRegistrationInfo Address);
    public record StoreRegistrationInfo(string Name, string Description, AddressRegistrationInfo Address, Time OpenTime, Time CloseTime, bool IsEnable);
    public record LoginedAccount(string UserName, string Password);
    public record ChangePasswordAccount(string OldPassword, string NewPassword);
    public record ResetPasswordAccount(string UserName, string NewPassword);
    public record VerifyResetPwAccount(string UserName, int Otp);
    public record Date(int Year, int Month, int Day);
    public record Time(int Hours, int Minute);
    public record RefreshToken(string Token);
}
