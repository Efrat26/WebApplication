using Logs.ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Client;

namespace WebApplication2.Models
{
    public class LogsModel
    {
        /// <summary>
        /// The client
        /// </summary>
        private IWebClient client;
        /// <summary>
        /// The logs
        /// </summary>
        private List<LogMessage> logs;
        public IWebClient ClientAdapter { get { return this.client; } set { this.client = value; } }
        public List<LogMessage> Logs { get { return this.logs; } set { this.logs = value; } }
        public LogsModel()
        {
            ClientAdapter = new ImageWebClient();
            if (!ClientAdapter.Client.IsConnected)
            {
                ClientAdapter.Connect();
            }
            this.logs = new List<LogMessage>();
            ClientAdapter.NotifyOnMessage += this.OnMessage;
            this.ClientAdapter.GetLogs();
        }
        public void OnMessage(String message)
        {
            {
                Console.WriteLine("in logs view model, got: " + message);

                if (message.Equals(Infrastructure.Enums.ResultMessgeEnum.Success.ToString()) ||
                message.Equals(Infrastructure.Enums.ResultMessgeEnum.Fail.ToString()))
                {
                    Console.WriteLine("in logs model, got: " + message);
                }
                else
                {
                    try
                    {
                        LogMessage log = LogMessage.FromJSON(message);
                        this.logs.Add(log);
                    } catch (Exception) { }
                }
            }
        }
    }
}