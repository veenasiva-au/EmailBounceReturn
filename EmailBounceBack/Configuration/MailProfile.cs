using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EmailBounceBack.Configuration
{
    [Serializable]
    public class MailProfile
    {
        public Guid MailboxGUID { get; set; }
        public Boolean Enabled { get; set; }


        public String ImapHost { get; set; }
        public int ImapPort { get; set; }
        public String ImapUserName { get; set; }
        public String ImapPassword { get; set; }
        public String ImapFolder { get; set; }
        private TimeSpan timeBetweenRetries;

        [XmlIgnore]
        public TimeSpan TimeBetweenRetries
        {
            get { return timeBetweenRetries; }
            set { timeBetweenRetries = value; }
        }

        [XmlElement(ElementName = "TimeBetweenRetries")]
        public long TimeBetweenRetriesTicks
        {
            get { return timeBetweenRetries.Ticks; }
            set { timeBetweenRetries = new TimeSpan(value); }
        }

        public int MaximumRetries { get; set; }

        public String ConnectionString { get; set; }
        public List<TableColumn> Table { get; set; }     
    }
}
