using System.Net;
using System.Net.Mail;
using System.Text;

namespace HR.Utils.Mail
{
    public class MailServe
    {
        private string Host { get; set; }
        private int Port { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string Subject { get; set; }
        private string Content { get; set; }
        private string MailFromAddress { get; set; }
        private string MailFromName { get; set; }
        private string[] MailToAddresses { get; set; }

        public MailServe(string host, int port, string userName, string password)
        {
            Host = host;
            Port = port;
            UserName = userName;
            Password = password;
        }

        public void SetSubject(string subject)
        {
            Subject = subject;
        }

        public void SetContent(string content)
        {
            Content = content;
        }

        public void SetFromAddress(string fromAddress)
        {
            MailFromAddress = fromAddress;
        }

        public void SetFromName(string fromName)
        {
            MailFromName = fromName;
        }

        public void SetToAddresses(string[] addresses)
        {
            MailToAddresses = addresses;
        }

        public void SendMail()
        {
            SmtpClient smtpClient = new()
            {
                Host = Host,
                Port = Port,
                UseDefaultCredentials = false,
                EnableSsl = true,
            };
            NetworkCredential MyCredentials = new(UserName, Password);
            smtpClient.Credentials = MyCredentials;

            MailMessage mail = new()
            {
                SubjectEncoding = Encoding.UTF8,
                Subject = Subject,
                IsBodyHtml = true,
                From = new MailAddress(MailFromAddress, MailFromName),
            };

            foreach (string Address in MailToAddresses)
            {
                mail.To.Add(new MailAddress(Address));
            }

            mail.Body = Content;

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception)
            {
            }
        }
    }
}
