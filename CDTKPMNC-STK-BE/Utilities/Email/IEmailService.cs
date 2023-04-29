namespace CDTKPMNC_STK_BE.Utilities.Email
{
    public interface IEmailService
    {
        public string EmailSender { get; set; }
        public string Password { get; set; }
        bool Send(string to, string subject, string html);
    }
}
