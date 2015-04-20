

using System;
using System.Collections.ObjectModel;
using System.Net.Mail;
using Ap.Common.Extensions;

namespace Ap.Mailer
{
    public class SmtpMailRelay
    {
        public event SendCompletedEventHandler SendCompleted;

        public bool Send(string to, string subject, string body)
        {
            var addresses = new MailAddressCollection();
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException("to");
            }

            addresses.Add(to);

            return SendMail(addresses, subject, body, null);
        }

        public bool SendMail(MailAddress to, string subject, string body)
        {
            var addresses = new MailAddressCollection { to };

            return SendMail(addresses, subject, body, null);
        }

        public bool SendMail(MailAddress to, string subject, string body, Attachment attachment)
        {
            var addresses = new MailAddressCollection { to };
            var attachments = new Collection<Attachment> { attachment };
            return SendMail(addresses, subject, body, attachments);
        }

        public bool SendMail(MailAddressCollection to, string subject, string body)
        {
            return SendMail(to, subject, body, null);
        }

        public bool SendMail(MailAddressCollection to, string subject, string body, Collection<Attachment> attachments)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.AddRange(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Attachments.AddRange(attachments);

            return SendMail(mailMessage);
        }

        public bool SendMail(MailMessage message)
        {
            using (var client = new SmtpClient())
            {

                if (SendCompleted != null)
                {
                    client.SendCompleted += SendCompleted;
                }

#if DEBUG
                message.To.Clear();
                message.To.Add(new MailAddress("codebased@hotmail.com"));
                client.Send(message);
#else
            client.Send(message);
#endif

                message.Attachments.ForEach(attachment => attachment.Dispose());
            }

            return true;
        }
    }
}