using System;
using SvnTimeService.Server.Contracts;

namespace SvnTimeService.Server.Core
{
    public class GuiLogger : IServiceLogger
    {
        public event EventHandler<string> SystemInfoLogged;
        public event EventHandler<string> RequestInfoLogged; 
        public void LogSystemInfo(string log)
        {
            OnSystemInfoLogged(log);
        }

        public void LogRequestInfo(string log)
        {
            OnRequestInfoLogged(log);
        }

        protected virtual void OnSystemInfoLogged(string logMsg)
        {
            SystemInfoLogged?.Invoke(this, logMsg);
        }


        protected virtual void OnRequestInfoLogged(string e)
        {
            RequestInfoLogged?.Invoke(this, e);
        }
    }
}