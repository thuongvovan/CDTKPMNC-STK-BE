using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using CDTKPMNC_STK_BE.Models;

//string from = "cd.tkpmnc@outlook.com";
//string password = "Mnbvcxz@7654321";

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class EmailService
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public SecureSocketOptions SecureSocketOption { get; set; }
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailInfo = _configuration.GetSection("Email");
            Email = emailInfo.GetValue<string>("Email");
            Password = emailInfo.GetValue<string>("Password");
            SMTPServer = emailInfo.GetValue<string>("SMTPServer");
            Port = emailInfo.GetValue<int>("Port");
            SecureSocketOption = emailInfo.GetValue<SecureSocketOptions>("SecureOption");
        }

        public bool Send(string to, string subject, string html)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(Email));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send emailInfo
                using var smtp = new SmtpClient();
                // smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtp.Connect(SMTPServer, Port, SecureSocketOption);
                smtp.Authenticate(Email, Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


