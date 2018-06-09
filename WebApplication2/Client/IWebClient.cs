using ex2_AP2.Settings.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Client
{
    public delegate void GotMessage(String message);
    public interface IWebClient
    {
        event GotMessage NotifyOnMessage;
        IClient Client { get; set; }
        void Connect();
        void MessageRecieved(String message); 
        void GetAppConfig();
        void GetLogs();
        bool RemoveHandler(String handlerName);
    }
}
