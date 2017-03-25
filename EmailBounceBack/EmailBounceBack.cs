using System;
using System.ServiceProcess;
using EmailBounceBack.Helpers;
using log4net.Config;
using EmailBounceBack.Core;

namespace EmailBounceBack
{
    partial class EmailBounceBack : ServiceBase
    {
        EmailMonitor monitor = null;
        EmailResender resender = null;
        public EmailBounceBack()
        {
            XmlConfigurator.Configure(); // Initializing logger
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Subscribe to the AppDomain UnhandledException handler to allow us
                // to perform any cleanup and logging before stopping the service
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                //monitor = new EmailMonitor();
                resender = new EmailResender();

                LogProvider.Log(GetType()).Info("EmailBounceBack Started.");
            }
            catch (Exception e)
            {
                LogProvider.Log(GetType()).Fatal(e);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogProvider.Log(GetType()).Debug("Stopping service.");
                if (monitor != null)
                {
                    monitor.Dispose();
                    monitor = null;
                }
                if(resender!= null)
                {
                    resender.Dispose();
                    resender = null;
                }
                LogProvider.Log(GetType()).Debug("Stopped service.");
            }
            catch (Exception e)
            {
                LogProvider.Log(GetType()).Warn(e);
            }

            LogProvider.Log(GetType()).Info("EmailBounceBack Stopped.");
        }
        #region private methods
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Handle the Unhandled Exception here, basically just log the
            // exception (local and event log) and stop the service gracefully
            LogProvider.Log(GetType()).Fatal(e.ExceptionObject);
        }
        #endregion
    }
}
