using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EmailBounceBack.Configuration;
using EmailBounceBack.DataLayer;

namespace EmailBounceBack.Helpers
{
    static class Settings
    {
        #region Properties
        
        public static TimeSpan EmailMonitorInterval { get; private set; }
        public static TimeSpan EmailResenderInterval { get; private set; }
        public static int ConcurrencyLevel { get; private set; }
        public static String DefaultEscalationEmail { get; private set; }
        
        public static Dictionary<Guid, MailProfile> MailboxProfiles { get; private set; }
        #endregion

        #region Constructor
        static Settings()
        {
            #region General Settings

            using (var ctx = new EmailBounceBackDataContext())
            {
                try
                {
                    EmailMonitorInterval = TimeSpan.Parse(ctx.Settings.Single(s => s.Name == "Interval.EmailMonitor").Value);
                }
                catch
                {
                    EmailMonitorInterval = TimeSpan.FromMinutes(1);
                }

                try
                {
                    EmailResenderInterval = TimeSpan.Parse(ctx.Settings.Single(s => s.Name == "Interval.EmailResender").Value);
                }
                catch
                {
                    EmailResenderInterval = TimeSpan.FromMinutes(5);
                }

                
                try
                {
                    ConcurrencyLevel = int.Parse(ctx.Settings.Single(s => s.Name == "ConcurrencyLevel").Value);
                }
                catch
                {
                    ConcurrencyLevel = Environment.ProcessorCount;
                }

                
                var setting = ctx.Settings.SingleOrDefault(s => s.Name == "DefaultEscalationEmail");
                DefaultEscalationEmail = (setting == null) ? null : setting.Value;

                if (String.IsNullOrWhiteSpace(DefaultEscalationEmail))
                    throw new Exception("DefaultEscalationEmail setting must contain a valid email address.");
            }

            #endregion
            #region Mailboxes
            MailboxProfiles = new Dictionary<Guid, MailProfile>();
            LoadMailboxProfiles();
            #endregion
        }
        #endregion

        #region Load Mailbox Profiles
        public static void LoadMailboxProfiles()
        {
            MailboxProfiles.Clear();
            XmlSerializer serializer = new XmlSerializer(typeof(MailProfile));

            using (var ctx = new EmailBounceBackDataContext())
            {
                foreach (var mailbox in ctx.MailBoxes)
                {
                    using (TextReader reader = new StringReader(mailbox.ProfileObject))
                    {
                        var profile = (MailProfile)serializer.Deserialize(reader);
                        if (profile.Enabled)
                            MailboxProfiles.Add(mailbox.MailboxGUID, profile);
                    }
                }
            }
        }
        #endregion
    }
}
