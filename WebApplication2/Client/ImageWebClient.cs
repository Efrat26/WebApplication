using ex2_AP2.Settings.Client;
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
        public IClient Client { get { return this.client; }set { this.client = value; } }
        public ImageWebClient()
        {
            this.Client = BasicClient.Instance;
            
        }
       public void Connect()
        {
            this.Client.connect(Communication.CommunicationDetails.IP,
                 Communication.CommunicationDetails.port);
        }
    }
}
