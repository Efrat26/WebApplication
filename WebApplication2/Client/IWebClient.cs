using ex2_AP2.Settings.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Client
{
    public delegate void GotMessage(String message);
    /// <summary>
    /// interface for a web client
    /// </summary>
    public interface IWebClient
    {
        /// <summary>
        /// Occurs when got a message.
        /// </summary>
        event GotMessage NotifyOnMessage;
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        IClient Client { get; set; }
        /// <summary>
        /// Connects to server.
        /// </summary>
        void Connect();
        /// <summary>
        /// invokes the event NotifyOnMessage
        /// </summary>
        /// <param name="message">The message.</param>
        void MessageRecieved(String message);
        /// <summary>
        /// Gets the application configuration by sending command to server.
        /// </summary>
        void GetAppConfig();
        /// <summary>
        /// Gets the logs by sending command to server.
        /// </summary>
        void GetLogs();
        /// <summary>
        /// Removes the handler by sending command to server.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <returns></returns>
        bool RemoveHandler(String handlerName);
    }
}
