using ex2_AP2.Settings.Client;
using Infrastructure.Enums;
using Logs.AppConfigObjects;
using Logs.Controller.Handlers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebApplication2.Client
{
    /// <summary>
    /// an image web client
    /// </summary>
    /// <seealso cref="WebApplication2.Client.IWebClient" />
    public sealed class ImageWebClient : IWebClient
    {
        private IClient client;
        /// <summary>
        /// Occurs when got a message.
        /// </summary>
        public event GotMessage NotifyOnMessage;
        /// <summary>
        /// Gets or sets the basic client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
       public IClient Client { get { return this.client; } set { this.client = value; } }
        /// <summary>
        /// bool that tells if we should wait for suuccess message (for example if requested to 
        /// delete a handler)
        /// </summary>
        private bool waitForSuccess;
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageWebClient"/> class.
        /// </summary>
        public ImageWebClient()
        {
            this.waitForSuccess = false;
            this.Client = BasicClient.Instance;
            this.Client.GotMessage += this.MessageRecieved;
            //Client.GotMessage += this.MessageRecieved;

        }
        /// <summary>
        /// Connects to server.
        /// </summary>
        public void Connect()
        {
            this.Client.connect(Communication.CommunicationDetails.IP,
                 Communication.CommunicationDetails.port);
            Client.Listen();
        }
        /// <summary>
        /// sending command to server to get the application configuration.
        /// </summary>
        public void GetAppConfig()
        {
            String appConfigCommand = ((int)CommandEnum.GetConfigCommand).ToString();
            Client.write(appConfigCommand);
        }
        /// <summary>
        /// invokes the message recieved event
        /// if waitForSuccess is true and it gets success it changes it
        /// </summary>
        /// <param name="message">The message.</param>
        public void MessageRecieved(string message)
        {
            if (this.waitForSuccess && message.Equals(ResultMessgeEnum.Success.ToString())) {
                this.waitForSuccess = false;
            }
            this.NotifyOnMessage?.Invoke(message);
        }
        /// <summary>
        /// send command to server to get log files
        /// </summary>
        public void GetLogs()
        {

            String logCommand = ((int)CommandEnum.LogCommand).ToString();
            Client.write(logCommand);
        }
        /// <summary>
        /// sends command to remove the handler specified
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <returns></returns>
        public bool RemoveHandler(String handlerName)
        {
            this.waitForSuccess = true;
            HandlerToClose h = new HandlerToClose(handlerName);
            String jobject = h.ToJSON();
            int message = (int)CommandEnum.CloseHandler;
            String newMessage = message.ToString() + jobject;
            client.write(newMessage);
            while(!this.waitForSuccess) { };
            return true;
        }
    }
}
