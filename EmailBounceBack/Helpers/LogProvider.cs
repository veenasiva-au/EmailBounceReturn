using System;
using log4net;
using System.Reflection;

namespace EmailBounceBack.Helpers
{
    public class LogProvider
    {
        public static ILog Log()
        {
            return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static ILog Log(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}
