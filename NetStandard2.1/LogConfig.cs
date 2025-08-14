using System;

namespace AdjustSdk.NetStandard
{
    public static class LogConfig
    {
        public static void SetupLogging(Action<string> logDelegate, LogLevel? logLevel = null)
        {
            var logger = AdjustFactory.Logger;
            if (logger.IsLocked) { return; }

            logger.LogDelegate = logDelegate;
            if (logLevel.HasValue)
            {
                logger.LogLevel = logLevel.Value;
            }
        }
    }
}
