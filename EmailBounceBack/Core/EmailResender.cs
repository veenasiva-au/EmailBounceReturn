using System;
using System.Threading;
using System.Threading.Tasks;
using EmailBounceBack.Helpers;
using System.Collections.Generic;
using EmailBounceBack.DataLayer;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Aspose.Email.Mail;
using System.IO;

namespace EmailBounceBack.Core
{
    public class EmailResender :IDisposable
    {
        private System.Timers.Timer timer;
        private AutoResetEvent autoEvent;
        double interval = 3000;

        #region Member Fields

        private MailMessage message;
        //private MailboxProfile profile;
        private ResendEmail email;
        #region interface implementation
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Elapsed -= timer_Elapsed;
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

        }
        #endregion
        #endregion

        #region Static Methods
        static int count = 0;

        public static int Queued { get { return count; } }
        #endregion
        public EmailResender()
        {
            interval = Properties.Settings.Default.Interval;
            autoEvent = new AutoResetEvent(true);
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Interval = interval;
            timer.Start();
        }
        private void timer_Elapsed(object sender, EventArgs e)
        {
            if (autoEvent.WaitOne(TimeSpan.Zero, false))
            {
                try
                {
                    LogProvider.Log(GetType()).Debug("Timer Thread - Started");
                   
                    // Email Resend
                    while (ResendEmail() != 0)
                    {
                        if (!timer.Enabled)
                            break;
                    }

                }
                catch(Exception ex)
                {
                    LogProvider.Log(GetType()).Error(ex);
                }
                finally
                {
                    autoEvent.Set();

                    LogProvider.Log(GetType()).Debug("Timer Thread - Released");
                }
            }
        }
        private int ResendEmail()
        {
            int count = 0;

            //Get emails that requires a resend
            //This includes only emails where retry attempt max count not reached
            var emailstoresend = GetEmailQueue();
            if (emailstoresend.Any())
            {
                LogProvider.Log(GetType()).Info(String.Format("Processing {0} email{1}...", emailstoresend.Count(), emailstoresend.Count() == 1 ? "" : "s"));
                //try resnd in parallel
                Resend resend=new Resend();
                Parallel.ForEach(emailstoresend, email => { resend.ResendEmail(email);  });
                //foreach (var email in emailstoresend)
                //{
                //    // Throttle throughput
                //    while (Queued >= Settings.ConcurrencyLevel)
                //    {
                //        if (!timer.Enabled)
                //            break;

                //        Thread.Sleep(100);
                //    }
                //    if (!timer.Enabled)
                //        break;
                //    LogProvider.Log(GetType()).Info(String.Format("Processing Email-ID {0}...", email.EmailID));

                //    // Can't proceed if the mailbox profile isn't found
                //    if (!Settings.MailboxProfiles.ContainsKey(email.MailboxGUID.GetValueOrDefault()))
                //    {
                //        var message = email.MailboxGUID.HasValue ? String.Format("Mailbox Profile not found (MailboxGUID = {0}).", email.MailboxGUID) : "Mailbox Profile not found.";

                //        // Log the error
                //        LogProvider.Log(GetType()).Error(message);

                //        // Update the email status
                //        UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Escalate", false));

                //        continue;
                //    }

                //    // Get the mailbox profile required for the conversion   
                //    var profile = Settings.MailboxProfiles[email.MailboxGUID.Value];

                //    //Resend email to default 
                //    Emailer emailer = new Emailer(profile.ImapHost, profile.ImapPort);
                //    using (EmailBounceBackController controller = new EmailBounceBackController())
                //    {
                //        UpdateStatus(EmailStatus.InProgress, email);
                //        var fromaddress = controller.getSettingValue("Email_From", profile.ConnectionString);
                //        var toaddress = controller.getSettingValue("DefaultMailbox", profile.ConnectionString);
                //        //ADD resend to subject
                //        var ResendEmailSubject = email.OriginalEmailSubject.Insert(email.OriginalEmailSubject.IndexOf('(') + 1, "RESEND-");
                //        var ResendEmailBody = controller.getSettingValue("Dripfeed_Email_Body", profile.ConnectionString);
                //        try
                //        {
                //            emailer.SendEmail(fromaddress
                //               , toaddress
                //               , ResendEmailSubject
                //               , ResendEmailBody
                //               , Directory.GetFiles(Path.GetDirectoryName(email.OriginalDocumentPath), Path.GetFileName(email.OriginalDocumentPath)));
                //        }
                //        catch (SmtpException ex)
                //        {
                //            //Connection Failure
                //            // Create the error xml based on the exception thrown
                //            var message = RemoveInvalidXmlChars(ex.ToString());
                //            UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Retry", true));
                //        }
                //        catch (Exception ex)
                //        {
                //            // Create the error xml based on the exception thrown
                //            var message = RemoveInvalidXmlChars(ex.ToString());
                //            UpdateStatus(EmailStatus.Error, email, GetErrorXml(message, "Retry", true));
                //        }
                //        finally
                //        {
                //            Interlocked.Increment(ref count);
                //        }
                //    }

                //}
            }
            return count;
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
        private IEnumerable<ResendEmail> GetEmailQueue()
        {
            List<ResendEmail> emails = new List<ResendEmail>();
            using (var ctx = new EmailBounceBackDataContext())
            {
                foreach (var profile in Settings.MailboxProfiles.Values.Where(m => m.Enabled))
                {
                    var retryTime = DateTime.Now.Subtract(profile.TimeBetweenRetries);
                    emails.AddRange((from email in ctx.ResendEmails
                                     where (email.MailboxGUID == profile.MailboxGUID) &&
                                     (email.InProgress == null || email.InProgress == false) &&
                                     ((email.Status == null) ||
                                     (email.Status == "Error" 
                                     && (email.EndTime == null || email.EndTime.Value < retryTime)
                                     &&(email.RetryCount.GetValueOrDefault()<=profile.MaximumRetries)))
                                     select email));

                }
            }
            return emails.OrderBy(e => e.EmailID);
        }
        private void UpdateStatus(EmailStatus status, ResendEmail email)
        {
            UpdateStatus(status, email, null);
        }
        private void UpdateStatus(EmailStatus status, ResendEmail email,XElement errors)
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
        private String RemoveInvalidXmlChars(String xml)
        {
            if (String.IsNullOrWhiteSpace(xml))
                return xml;

            return new String(xml.Where(c => XmlConvert.IsXmlChar(c)).ToArray());
        }
    }
}
