using System;
using System.Diagnostics;
using System.Threading;
using EmailBounceBack.Helpers;
using EmailBounceBack.Configuration;
using EmailBounceBack.DataLayer;
using Decipha.Net.Mail;
using Aspose.Email;
using Aspose.Email.Mail;
using Aspose.Email.Mail.Bounce;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace EmailBounceBack.Core
{
    public class EmailMonitor : IDisposable
    {
        #region Fields
        private int threadLockCount;
        private DateTime currentDate;
        private System.Timers.Timer timer;
        private AutoResetEvent autoEvent;
        double interval = 1000;
        #endregion
        public EmailMonitor()
        {
            this.threadLockCount = 0;
           
            this.autoEvent = new AutoResetEvent(true);

            this.timer = new System.Timers.Timer();
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            this.timer.Interval = interval;
            this.timer.Start();
        }
        #region Event Handlers
        private void timer_Elapsed(object sender, EventArgs e)
        {
            //if (autoEvent == null)
            //{
            //    if (timer != null)
            //    {
            //        this.autoEvent = new AutoResetEvent(true);
            //    }
            //}
            if (autoEvent.WaitOne(TimeSpan.Zero, false))
            {
                threadLockCount = 0;
                try
                {
                    LogProvider.Log(GetType()).Debug("Timer Thread - Acquired");

                    // Reset the timer interval
                    interval = Properties.Settings.Default.Interval;
                    //Download mails from mailbox
                    while (DownloadEmail() != 0)
                    {
                        if (!timer.Enabled)
                            break;
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    LogProvider.Log(GetType()).Fatal(ex);
                    RestartService();
                }
                catch (Exception ex)
                {
                    LogProvider.Log(GetType()).Error(ex);
                }
                finally
                {
                    autoEvent.Set();

                    LogProvider.Log(GetType()).Debug("Timer Thread - Released");
                }
            }
            else
            {
                if (++threadLockCount == 5)
                    LogProvider.Log(GetType()).Warn("Timer Thread - Locked");
                else
                    LogProvider.Log(GetType()).Debug("Timer Thread - Locked");
            }
        }
        private void RestartService()
        {
            try
            {
                var serviceName = String.Format("Email Import{0}", Program.EnableProcess ? "" : " - Collect");
                LogProvider.Log(GetType()).Fatal(String.Format("{0} service restarting due to memory overuse.", serviceName));
                Process.Start("cmd.exe", String.Format("/C NET STOP \"{0}\" & NET START \"{0}\"", serviceName));

                // Wait till we receive the stop signal (up to 10 seconds)
                for (int i = 0; i < 100; i++)
                {
                    if (!timer.Enabled)
                        break;

                    Thread.Sleep(100);
                }
            }
            catch
            { }
        }
        #endregion
        #region Download Email
        private int DownloadEmail()
        {
            int count = 0;
            // Get mailboxes defined
            var profiles = from profile in Settings.MailboxProfiles.Values
                           select profile;
            
            foreach (var profile in profiles)
            {
                // If the timer has been stopped stop the processing loop
                if (!timer.Enabled)
                    break;

                // Ignore if no imap host is defined
                if (String.IsNullOrWhiteSpace(profile.ImapHost))
                    continue;
                LogProvider.Log(GetType()).Debug("Processing mailbox {0} ..."+profile.ImapUserName);

                try {
                    // Create an imap session (auto connect/login/select)
                    using (Imap imap = new Imap(profile.ImapHost, profile.ImapPort, SecurityOptions.None, profile.ImapUserName, profile.ImapPassword, profile.ImapFolder))
                    {
                        // Download the actual mail message
                        count += DownloadEmail(imap, profile);

                        // Expunge any deleted messages
                        imap.ExpungeMessages();
                    }
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    LogProvider.Log(GetType()).Error(ex.InnerException);
                }
            }
            return count;
        }
        private int DownloadEmail(Imap imap, MailProfile profile)
        {
            int count = 0;
            // Build the MailQuery
            var query = new MailQuery( "('Deleted' = 'False')" );

            var messages = imap.ListMessages(query);
            foreach(var message in messages)
            {
                MailMessage msg = null;
         
                msg = imap.FetchMessage(message.UniqueId);
                var filtertext = Properties.Settings.Default.Filtertext.Split(',').Where(x=> x.Trim().Length >0);
                if (filtertext.Any(x => msg.Subject.ToLower().Trim().Contains(x.ToLower().Trim())))
                {
                    LogProvider.Log(GetType()).Info(string.Format("Bounce Message Found : {0} Subject contains filter text : {1}" ,msg.Subject, filtertext.Any(x => msg.Subject.ToLower().Contains(x.ToLower()))));
                    //if a message is found check subject has documentid
                    LogProvider.Log(GetType()).Debug("Validating Mail Subject");
                    if(msg.Subject.ToLower().IndexOf("(resend")>0)
                    {

                    }
                    else if (msg.Subject.IndexOf('(') > 0)
                    {
                        int DocumentID = Convert.ToInt32(msg.Subject.Substring(msg.Subject.IndexOf('(') + 1, msg.Subject.IndexOf(')') - msg.Subject.IndexOf('(') - 1));
                        if (Validate(DocumentID, profile.ConnectionString))
                        {
                            LogProvider.Log(GetType()).Info(string.Format("Message with Document ID {0} found in origin system",DocumentID));
                            //insert in table for document to be resend
                            using (EmailBounceBackController controller = new EmailBounceBackController())
                            {
                                ResendEmail dr = new ResendEmail()
                                {
                                    MessageID=msg.MessageId,
                                    Subject=msg.Subject,
                                    DocumentID = DocumentID,
                                    OriginalDocumentPath = controller.getDocumentPath(DocumentID,profile.ConnectionString),
                                    OriginalEmailSubject=controller.getDocumentSubject(DocumentID,profile.ConnectionString),
                                    TimeStamp=DateTime.Now
                                };
                                controller.InsertDocumentResend(dr);
                            }
                        }

                        // Move the email to the archive (if this fails, but the download is complete this
                        // will just result in a duplicate next time round if the deleted flag is not set)
                        imap.MoveMessage(message.UniqueId, "ResendArchive", true, false);
                        // Increment the download count
                        count++;
                    }
                    else
                    { //The message is not undeliverable or automated response, move to Archive so it will not be picked agian
                      imap.MoveMessage(message.UniqueId, "Archive", true, false);
                    }
                }
                else
                {
                    //The message is not undeliverable or automated response, move to Archive so it will not be picked agian
                    imap.MoveMessage(message.UniqueId, "Archive", true, false);
                }
            }
            return count;
        }
        #endregion
        #region validation
        private bool Validate(int DocumentID,string ConnectionString)
        {
            //check if document id is valid
            GetDataContext(ConnectionString, "Documents");
            using (EmailBounceBackDataContext ctx = new EmailBounceBackDataContext(ConnectionString))
            {
                    var exists = ctx.Documents.Where(x => x.DocumentID == DocumentID).FirstOrDefault();
                if (exists == null)
                    return false;
            }
            return true;
        }
        
        public static EmailBounceBackDataContext GetDataContext(string CNN_STRING,string tableName)
        {
            
        // Get the .xml file into memory 
        //Stream ioSt = Assembly.GetExecutingAssembly()
        //           .GetManifestResourceStream("Documents.xml");
            XElement xe = XElement.Load(XmlReader.Create("Documents.xml"));
            // Replace the table name value in memory
            var tableElements = xe.Elements().AsQueryable().Where(e => e.Name.LocalName.Equals("Table"));
            foreach (var t in tableElements)
            {
                foreach(var col in t.Descendants().Where(x => x.Name.LocalName == "Column" && x.Name=="PDFFilePath"))
                {
                    XAttribute name = col.Attributes().FirstOrDefault(a => a.Name.LocalName == "Name");
                    name.Value = "PDFFilePath";
                }
                //var nameAttribute = t.Attributes().Where(a => a.Name.LocalName.Equals("Name"));
                //foreach (var a in nameAttribute)
                //{
                //    if (a.Value.Equals("dbo.Documents"))
                //    {
                //        a.Value = a.Value.Replace("Documents", tableName);
                        
                //    }
                //}
            }
            // Obtain and return the dynamic DataContext
            System.Data.Linq.Mapping.XmlMappingSource source = System.Data.Linq.Mapping.XmlMappingSource.FromXml(xe.ToString());
            return new EmailBounceBackDataContext(CNN_STRING, source);
        }
        #endregion
        #region IDisposable Implementation

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (autoEvent != null)
                    {
                        timer.Stop();
                        autoEvent.WaitOne();

                        timer.Dispose();
                        timer = null;

                        autoEvent.Close();
                        autoEvent = null;
                    }
                }

                disposed = true;
            }
        }

        #endregion
    }
    #region Imap Helper Class

    //public class Imap : Decipha.Net.Mail.Imap
    //{
    //    public Imap(MailProfile profile)
    //        : base(profile.ImapHost, profile.ImapPort, profile.ImapUserName, profile.ImapPassword, profile.ImapFolder)
    //    { }
    //}

    #endregion
}
