using System;
using System.Linq;
using EmailBounceBack.Helpers;
using System.Reflection;
using System.ServiceProcess;
using System.Configuration.Install;


namespace EmailBounceBack
{
    static class Program
    {
        public static Boolean EnableCollect;
        public static Boolean EnableProcess;
        static void Main(string[] args)
        {
            ParseStartArguments(args);
            var service = new EmailBounceBack();
            if (Environment.UserInteractive)
            {
                try
                {
                    switch (args.FirstOrDefault())
                    {
                        case "-i":
                        case "-install":
                            ManagedInstallerClass.InstallHelper(new String[] { Assembly.GetExecutingAssembly().Location });
                            break;

                        case "-u":
                        case "-uninstall":
                            ManagedInstallerClass.InstallHelper(new String[] { "/u", Assembly.GetExecutingAssembly().Location });
                            break;

                        case "-c":
                        case "-console":
                            Console.WriteLine("EmailBounceBack Console App");
                            Console.WriteLine("=================================="); service.GetType().InvokeMember("OnStart", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, service, new Object[] { null });
                            Console.WriteLine();
                            Console.WriteLine("Press any key to stop service...");
                            Console.ReadKey(true);
                            service.GetType().InvokeMember("OnStop", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, service, null);
                            break;
                        default:

                            Console.WriteLine("EmailBounceBack Console Help");
                            Console.WriteLine("===================================");
                            Console.WriteLine();
                            Console.WriteLine("-i or -install to install the application as a Windows service");
                            Console.WriteLine("-u or -uninstall to un-install already installed a Windows service");
                            Console.WriteLine("-c or -console to run the application as a console application");
                            Console.WriteLine("If you are running the application in debug, please add comandline arguments to the debug window in project properties");
                            Console.WriteLine("Press any key to exit...(or exit in 10 seconds)");
                            Reader.ReadLine(10000);
                            //Console.ReadKey(true);
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogProvider.Log().Error(e);
                }
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
        private static void ParseStartArguments(String[] args)
        {
            if (args != null)
            {
                foreach (var arg in args)
                {
                    switch (arg.ToLower())
                    {
                        case "-monitor":
                            EnableCollect = true;
                            break;

                        case "-resender":
                            EnableProcess = true;
                            break;
                    }
                }
            }

            if (EnableCollect == false && EnableProcess == false)
            {
                EnableCollect = true;
                EnableProcess = true;
            }
        }
    }
}
