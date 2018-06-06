using Logs.AppConfigObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Client;

namespace WebApplication2.Models
{
    public class ConfigModel
    {
        /// <summary>
        /// The client
        /// </summary>
        private IWebClient client;
        private String outputDirectoryPath;
        /// <summary>
        /// The source name
        /// </summary>
        private String sourceName;
        /// <summary>
        /// The log name
        /// </summary>
        private String logName;
        /// <summary>
        /// The thumbnail size
        /// </summary>
        private int thumbnailSize;
        /// <summary>
        /// The handlers
        /// </summary>
        private List<String> handlers;
        #region properties
        public string OutputDirectory
        {
            get
            {
                return this.outputDirectoryPath;
            }
            set
            {
                outputDirectoryPath = value;
                //NotifyPropertyChanged("OutputDirectory");

            }
        }
        public string SourceName
        {
            get
            {
                return this.sourceName;
            }
            set
            {
                sourceName = value;
                //NotifyPropertyChanged("SourceName");
            }
        }
        public string LogName
        {
            get
            {
                return this.logName;
            }
            set
            {
                logName = value;
                //NotifyPropertyChanged("LogName");
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return this.thumbnailSize;
            }
            set
            {
                thumbnailSize = value;
                //NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public List<String> Handlers
        {
            get
            {
                return this.handlers;
            }
            set
            {
                handlers = value;
            }
        }
        public IWebClient ClientAdapter { get { return this.client; } set { this.client = value; } }
        #endregion

        public ConfigModel()
        {
            
            ClientAdapter = new ImageWebClient();
            if (!ClientAdapter.Client.IsConnected)
            {
                ClientAdapter.Connect();
            }
            ClientAdapter.NotifyOnMessage += this.OnMessage;
            ClientAdapter.GetAppConfig();

        }
        public void OnMessage(String message)
        {
            if (message.Equals(Infrastructure.Enums.ResultMessgeEnum.Success.ToString())
           || message.Contains(Infrastructure.Enums.ResultMessgeEnum.Success.ToString()))
            {

            }
            else if (message.Equals(Infrastructure.Enums.ResultMessgeEnum.Fail.ToString()))
            {

            }
            else if (message != null)
            {
                try
                {
                    ImageServiceAppConfigItem initialConfig = ImageServiceAppConfigItem.FromJSON(message);
                    Console.WriteLine(initialConfig.OutputFolder);
                    this.OutputDirectory = initialConfig.OutputFolder;
                    Console.WriteLine(initialConfig.LogName);
                    this.LogName = initialConfig.LogName;
                    Console.WriteLine(initialConfig.SourceName);
                    this.SourceName = initialConfig.SourceName;
                    Console.WriteLine(initialConfig.ThumbnailSize);
                    this.ThumbnailSize = initialConfig.ThumbnailSize; string[] folders = initialConfig.Handlers.Split(';');
                    this.handlers = new List<string>();
                    foreach (String folder in folders)
                    {
                        //if (!this.handlers.Contains(folder))
                        //{
                            this.handlers.Add(folder);
                        //}
                    }
                }
                catch (Exception) { }
            }
        }
    }
}