// using MimeKit;
//using MimeKit.Text;
//using MailKit.Net.Smtp;
//using MailKit.Security;


using System.Net.Mail;
using System.Net;

//string from = "cd.tkpmnc@outlook.com";
//string password = "Mnbvcxz@7654321";

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class EmailService
    {
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string SMTPServer { get; set; }
        //public int Port { get; set; }
        //public SecureSocketOptions SecureSocketOption { get; set; }
        //private readonly IConfiguration _configuration;

        private readonly SmtpClient SmtpClient;

        public EmailService(IConfiguration configuration)
        {
            //_configuration = configuration;
            //var emailInfo = _configuration.GetSection("Email");
            //Email = emailInfo.GetValue<string>("Email");
            //Password = emailInfo.GetValue<string>("Password");
            //SMTPServer = emailInfo.GetValue<string>("SMTPServer");
            //Port = emailInfo.GetValue<int>("Port");
            //SecureSocketOption = emailInfo.GetValue<SecureSocketOptions>("SecureOption");

            SmtpClient = new SmtpClient("live.smtp.mailtrap.io", 25)
            {
                Credentials = new NetworkCredential("api", "65964ce2d50ed23ce1f136336a84aca7"),
                EnableSsl = true
            };
        }

        public bool Send(string to, string subject, string html)
        {
            try
            {
                //// create message
                //var email = new MimeMessage();
                //email.From.Add(MailboxAddress.Parse(Email));
                //email.To.Add(MailboxAddress.Parse(to));
                //email.Subject = subject;
                //email.Body = new TextPart(TextFormat.Html) { Text = html };

                //// send emailInfo
                //using var smtp = new SmtpClient();
                //// smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                //smtp.Connect(SMTPServer, Port, SecureSocketOption);
                //smtp.Authenticate(Email, Password);
                //smtp.Send(email);
                //smtp.Disconnect(true);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("info@vovanthuong.online"),
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);
                SmtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


