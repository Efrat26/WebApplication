using ex2_AP2.Settings.Client;
using Infrastructure.Enums;
using Logs.AppConfigObjects;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebApplication2.Client
{

    public sealed class ImageWebClient : IWebClient
    {
        private IClient client;

        public event GotMessage NotifyOnMessage;

        public IClient Client { get { return this.client; } set { this.client = value; } }


        public ImageWebClient()
        {
            this.Client = BasicClient.Instance;
            this.Client.GotMessage += this.MessageRecieved;
            //Client.GotMessage += this.MessageRecieved;

        }
        public void Connect()
        {
            this.Client.connect(Communication.CommunicationDetails.IP,
                 Communication.CommunicationDetails.port);
            Client.Listen();
        }
        //no need here
        public void GetAppConfig()
        {
            String appConfigCommand = ((int)CommandEnum.GetConfigCommand).ToString();
            Client.write(appConfigCommand);
        }

        public void MessageRecieved(string message)
        {
            this.NotifyOnMessage?.Invoke(message);
        }
        public void GetLogs()
        {

            String logCommand = ((int)CommandEnum.LogCommand).ToString();
            Client.write(logCommand);
        }
    }
}
