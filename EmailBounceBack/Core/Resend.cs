using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net.Mail;
using System.Threading;
using EmailBounceBack.DataLayer;
using EmailBounceBack.Helpers;

namespace EmailBounceBack.Core
{
    public class Resend
    {
        #region Public Properties
        //public ResendEmail email;
        #endregion

        #region Static Methods
        static int count = 0;
        public static int Queued { get { return count; } }
        #endregion

        #region Constructor
        public Resend()
        {
            //this.email = emailtoresend;
        }
        #endregion
        public void ResendEmail(ResendEmail email)
        {
                // Throttle throughput
                //while (Queued >= Settings.ConcurrencyLevel)
                //{
                //    if (!timer.Enabled)
                //        break;

                //    Thread.Sleep(100);
                //}
                //if (!timer.Enabled)
                //    break;
                LogProvider.Log(GetType()).Info(String.Format("Processing Email-ID {0}...", email.EmailID));

                // Can't proceed if the mailbox profile isn't found
                if (!Settings.MailboxProfiles.ContainsKey(email.MailboxGUID.GetValueOrDefault()))
                {
                    var message = email.MailboxGUID.HasValue ? String.Format("Mailbox Profile not found (MailboxGUID = {0}).", email.MailboxGUID) : "Mailbox Profile not found.";

                    // Log the error
                    LogProvider.Log(GetType()).Error(message);

                    // Update the email status
                    UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Escalate", false));

                    
                }
            else { 
                // Get the mailbox profile required for the conversion   
                var profile = Settings.MailboxProfiles[email.MailboxGUID.Value];

                //Resend email to default 
                Emailer emailer = new Emailer(profile.ImapHost, profile.ImapPort);
                using (EmailBounceBackController controller = new EmailBounceBackController())
                {
                    UpdateStatus(EmailStatus.InProgress, email);
                    var fromaddress = controller.getSettingValue("Email_From", profile.ConnectionString);
                    var toaddress = controller.getSettingValue("DefaultMailbox", profile.ConnectionString);
                    //ADD resend to subject
                    var ResendEmailSubject = email.OriginalEmailSubject.Insert(email.OriginalEmailSubject.IndexOf('(') + 1, "RESEND-");
                    var ResendEmailBody = controller.getSettingValue("Dripfeed_Email_Body", profile.ConnectionString);
                    try
                    {
                        emailer.SendEmail(fromaddress
                           , toaddress
                           , ResendEmailSubject
                           , ResendEmailBody
                           , Directory.GetFiles(Path.GetDirectoryName(email.OriginalDocumentPath), Path.GetFileName(email.OriginalDocumentPath)));
                    }
                    catch (SmtpException ex)
                    {
                        //Connection Failure
                        // Create the error xml based on the exception thrown
                        var message = RemoveInvalidXmlChars(ex.ToString());
                        UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Retry", true));
                    }
                    catch (Exception ex)
                    {
                        // Create the error xml based on the exception thrown
                        var message = RemoveInvalidXmlChars(ex.ToString());
                        UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Retry", true));
                    }
                    finally
                    {
                        Interlocked.Increment(ref count);
                    }
                }

            }
        }
   
        private void UpdateStatus(EmailStatus status, ResendEmail email)
        {
            UpdateStatus(status, email, null);
        }
        private void UpdateStatus(EmailStatus status, ResendEmail email, XElement errors)
        {
            int count = 0;

            while (true)
            {
                try
                {
                    using (EmailBounceBackDataContext ctx = new EmailBounceBackDataContext())
                    {
                        var eml = ctx.ResendEmails.Single(e => e.EmailID == email.EmailID);

                        if (status == EmailStatus.InProgress)
                        {
                            eml.InProgress = true;
                            eml.StartTime = DateTime.Now;
                            eml.EndTime = null;
                        }
                        else
                        {
                            eml.Status = status.ToString();
                            eml.RetryCount = eml.RetryCount.GetValueOrDefault() + 1;
                            eml.EndTime = DateTime.Now;
                            eml.InProgress = null;
                            eml.Errors = errors;
                        }

                        ctx.SubmitChanges();
                    }

                    break;
                }
                catch (Exception e)
                {
                    // Increment the retry counter
                    count++;

                    // Log the exception
                    //ConfigLogger.Instance.Log(count < 5 ? LogSeverity.Warning : LogSeverity.Error, email.EmailID, e);
                    LogProvider.Log(GetType()).Error(e);
                    // Retry up to 5 times
                    if (count >= 5)
                        break;

                    // Sleep for a fraction
                    Thread.Sleep(50);
                }
            }
        }
        private XElement GetErrorXml(String message, String action, Boolean allowRetry)
        {
            if (!String.IsNullOrWhiteSpace(message))
                message = new String(message.Where(c => XmlConvert.IsXmlChar(c)).ToArray());

            // Create the error xml based on the exception thrown
            return new XElement("Errors",
                       new XAttribute("retry", allowRetry ? "true" : "false"),
                       new XElement("Error",
                           new XAttribute("reason", "Unknown error."),
                           new XAttribute("message", message),
                           new XAttribute("action", action)
                           )
                       );
        }
        private String RemoveInvalidXmlChars(String xml)
        {
            if (String.IsNullOrWhiteSpace(xml))
                return xml;

            return new String(xml.Where(c => XmlConvert.IsXmlChar(c)).ToArray());
        }
    }
}
