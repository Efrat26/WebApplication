using ex2_AP2.Settings.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication2.Client
{
    /// <summary>
    /// a gui client - singleton
    /// </summary>
    /// <seealso cref="ex2_AP2.Settings.Client.IClient" />
    public sealed class BasicClient : IClient
    {
        #region members        
        /// <summary>
        /// determines if client is connected or not
        /// </summary>
        private bool connected;
        /// <summary>
        /// The end point for connection
        /// </summary>
        private IPEndPoint ep;
        /// <summary>
        /// The tcp client that actually works with the server
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// The stream
        /// </summary>
        private NetworkStream stream;
        /// <summary>
        /// The reader stream
        /// </summary>
        private BinaryReader reader;
        /// <summary>
        /// The writer stream
        /// </summary>
        private BinaryWriter writer;
        /// <summary>
        /// The instance (singleton)
        /// </summary>
        private static volatile BasicClient instance;
        private static object syncRoot = new Object();

        public event MessageRecieved GotMessage;

        public static BasicClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BasicClient();
                    }
                }

                return instance;
            }
        }
        #endregion members        
        /// <summary>
        /// Connects the specified ip.
        /// </summary>
        /// <param name="IP">The ip.</param>
        /// <param name="port">The port.</param>
        public void connect(String IP, int port)
        {

            ep = new IPEndPoint(IPAddress.Parse(IP), port);
            client = new TcpClient();
            try
            {
                client.Connect(ep);
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                if (client.Connected)
                {
                    this.IsConnected = true;
                    Console.WriteLine("You are connected");
                    Console.WriteLine("IsConnected: " + IsConnected);
                }
            }
            catch (Exception)
            {
                this.IsConnected = false;
            }
        }
        /// <summary>
        /// Determines whether client is connected.
        /// </summary>
        /// <returns>
        /// <c>true</c> if client is connected; otherwise, <c>false</c>.
        /// </returns>
        public bool isConnected()
        {
            if (client != null && client.Connected)
            {
                this.IsConnected = true;
                Console.WriteLine("in isconnected IsConnected: " + IsConnected);
                return true;
            }
            this.IsConnected = false;
            return false;
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get { return this.connected; } set { this.connected = value; } }
        /// <summary>
        /// Disconnects client.
        /// </summary>
        public void disconnect()
        {
            writer.Close();
            reader.Close();
            stream.Close();
            client.Close();
        }
        /// <summary>
        /// Reads data.
        /// </summary>
        /// <returns>
        /// the data read
        /// </returns>
        public String read()
        {
            String result;
            try
            {

                result = reader.ReadString();
                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }
        /// <summary>
        /// Writes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void write(String command)
        {
            //if (!client.Connected) { client.Connect(ep); }
            try
            {

                // Send data to server
                writer.Write(command);
                writer.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("error in writing to client: " + e.ToString());
            }
            finally
            {
                //writer.Dispose();
                //writer.Close();
            }

        }
        /// <summary>
        /// Listens to incoming messages.
        /// </summary>
        public void Listen()
        {
            Boolean stop = false;
            String message;
            Task task = new Task(() =>
            {
                while (!stop)
                {
                    message = this.read();
                    if (message != null)
                    {
                        this.GotMessage?.Invoke(message);
                    }
                }

            }); task.Start();
        }
    }
}