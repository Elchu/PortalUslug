using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PortalUslug.Helpers
{
    public class MailHelper
    {
        private static SmtpClient _smtpClient;

        static MailHelper()
        {
            _smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Wysłanie e-maila do pojedynczego odbiorcy.
        /// </summary>
        /// <param name="recipientAddress">Adres e-mail odbiorcy.</param>
        /// <param name="recipient">Nazwa odbiorcy.</param>
        /// <param name="subject">Temat e-maila.</param>
        /// <param name="news">Treść e-maila.</param>
        public static void SendEmail(string recipientAddress, string recipient, string subject, string news)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("PortalUslug@gmail.com", "Portal Usług", Encoding.UTF8);
                mail.To.Add(new MailAddress(recipientAddress, recipient, Encoding.UTF8));
                mail.Subject = subject;
                mail.Body = news;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.High;

                _smtpClient.Send(mail);
            }
        }

        /// <summary>
        /// Wysłanie e-maila do wielu odbiorców.
        /// </summary>
        /// <param name="recipientsAddresses">Lista adresów e-mail odbiorców.</param>
        /// <param name="subject">Temat e-maila.</param>
        /// <param name="news">Treść e-maila.</param>
        public static void SendEmail(List<string> recipientsAddresses, string subject, string news)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("PortalUslug@gmail.com", "Portal Usług", Encoding.UTF8);

                foreach (string adres in recipientsAddresses)
                {
                    mail.Bcc.Add(new MailAddress(adres));
                }

                mail.Subject = subject;
                mail.Body = news;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.High;

                _smtpClient.Send(mail);
            }
        }

    }
}