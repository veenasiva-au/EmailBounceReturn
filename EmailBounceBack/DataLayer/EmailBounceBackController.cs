using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailBounceBack.Configuration;


namespace EmailBounceBack.DataLayer
{
    public class EmailBounceBackController :IDisposable
    {
       
        public EmailBounceBackController()
        {

        }
       
        public  void InsertDocumentResend(ResendEmail dr)
        {
            using (EmailBounceBackDataContext ctx = new EmailBounceBackDataContext())
            {
                ctx.ResendEmails.InsertOnSubmit(dr);
                ctx.SubmitChanges();
            }
        }
        public string getDocumentSubject(int DocumentID, string ConnectionString)
        {
            using (EmailBounceBackDataContext context = new EmailBounceBackDataContext(ConnectionString))
            {
                return context.Documents.FirstOrDefault(x => x.DocumentID == DocumentID).Subject;
            }
        }
        public string getDocumentPath(int DocumentID, string ConnectionString)
        {
            using (EmailBounceBackDataContext context = new EmailBounceBackDataContext(ConnectionString))
            {
                return context.Documents.FirstOrDefault(x=> x.DocumentID==DocumentID).DocumentPath;
            }
        }
        public string getSettingValue(string name, string ConnectionString)
        {
            using (EmailBounceBackDataContext context = new EmailBounceBackDataContext(ConnectionString))
            {
                return context.Settings.Where(s => s.Name == name).FirstOrDefault().Value;
            }
        }
        #region Interface Implementation
        public void Dispose()
        { }
        #endregion
    }
}
