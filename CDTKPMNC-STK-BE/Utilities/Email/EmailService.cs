﻿using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using CDTKPMNC_STK_BE.Models;

namespace CDTKPMNC_STK_BE.Utilities.Email
{
    public class EmailService:IEmailService
    {
        public string EmailSender { get; set; }
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public SecureSocketOptions SecureSocketOption { get; set; }

        private readonly IConfiguration _configuration;

        // public EmailService(string from = "cd.tkpmnc@outlook.com", string password = "Mnbvcxz@7654321")
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var emailInfo = _configuration.GetSection("Email");
            EmailSender = emailInfo.GetValue<string>("Email");
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
                email.From.Add(MailboxAddress.Parse(EmailSender));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send emailInfo
                using var smtp = new SmtpClient();
                // smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtp.Connect(SMTPServer, Port, SecureSocketOption);
                smtp.Authenticate(EmailSender, Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
            
        }

        public void SendRegisterOTP(EndUserAccount userAccount)
        {
            string subject = "Your verification code for register account CĐ-TKPMNC";
            string html = @$"
                Dear {userAccount.Name},<br/>
                <br/>
                Your OTP is {userAccount.OTP?.RegisterOTP}.<br/>
                <br/>
                Please enter this code on our application to verify your account.<br/>
                <br/>
                Thanks you,<br/>
                Thương - Khôi - Sơn
                ";
            if (userAccount.Account is not null)
            {
                Send(userAccount.Account, subject, html);
            }
        }

        public void SendResetPasswordOTP(EndUserAccount userAccount)
        {
            string subject = "Your verification code for reset password CĐ-TKPMNC";
            string html = @$"
                Dear {userAccount.Name},<br/>
                <br/>
                Your OTP is {userAccount.OTP?.ResetPasswordOTP}.<br/>
                <br/>
                Please enter this code on our application to verify your password changed.<br/>
                <br/>
                Thanks you,<br/>
                Thương - Khôi - Sơn
                ";
            if (userAccount.Account is not null)
            {
                Send(userAccount.Account, subject, html);
            }
        }
    }
}


/*
string from = "cd.tkpmnc@outlook.com";
string password = "Mnbvcxz@7654321";
 */