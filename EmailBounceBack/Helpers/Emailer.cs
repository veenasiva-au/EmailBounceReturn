using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace EmailBounceBack.Helpers
{
    public class Emailer
    {
        SmtpClient smtpClient;

        public Emailer()
        {
            smtpClient = new SmtpClient();
        }

        public Emailer(string serverAddress, int serverPort)
        {
            smtpClient = new SmtpClient(serverAddress, serverPort);
        }



        public void SendEmail(string from, string to, string subject, string body)
        {
            SendEmail(from, to, subject, body, new string[] { });
        }

        public void SendEmail(string from, string to, string cc, string subject, string body)
        {
            SendEmail(from, to, cc, subject, body, new string[] { });
        }

        public void SendEmail(string from, string to, string cc, string bcc, string subject, string body)
        {
            SendEmail(from, to, cc, bcc, String.Empty, subject, body, new string[] { });
        }

        public void SendEmail(string from, string to, string subject, string body, string[] attachmentFileNames)
        {
            SendEmail(from, to, String.Empty, String.Empty, String.Empty, subject, body, attachmentFileNames);
        }

        public void SendEmail(string from, string to, string cc, string subject, string body, string[] attachmentFileNames)
        {
            SendEmail(from, to, cc, String.Empty, String.Empty, subject, body, attachmentFileNames);
        }

        public void SendEmail(string from, string to, string cc, string bcc, string replyTo, string subject, string body, string[] attachmentFileNames)
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(from);

                //Must have a 'to' address
                string[] recipients = to.Split(new char[] { ',', ';' });
                foreach (string recipient in recipients)
                {
                    //Check if we have any spaces
                    if (!string.IsNullOrEmpty(recipient))
                    {
                        string recipientTrimmed = recipient.Trim();
                        if (recipientTrimmed.Length > 0)
                        {
                            message.To.Add(recipientTrimmed);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(cc))
                {
                    message.CC.Add(cc);
                }
                if (!String.IsNullOrEmpty(bcc))
                {
                    message.Bcc.Add(bcc);
                }
                if (!String.IsNullOrEmpty(replyTo))
                {
                    message.ReplyTo = new MailAddress(replyTo);
                }
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                foreach (var attachmentFileName in attachmentFileNames)
                {
                    Attachment attachment = new Attachment(attachmentFileName);

                    message.Attachments.Add(attachment);
                }

                smtpClient.Send(message);
            }
        }
    }
}
