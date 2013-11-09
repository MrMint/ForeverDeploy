using ForeverDeploy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ForeverDeploy.Utilities
{
	/// <summary>
	/// Handles monitoring server logs, and updating the ServerStatusManager to server status changes
	/// </summary>
	public class ServerLogMonitor
	{
		//Monitor task
		private Task monitor;

		//Stream/Reader variables
		private FileStream logFileStream;
		private StreamReader logFileReader;

		//Represents current monitor state
		private bool isMonitoring;

		//Public accessor for isMonitoring
		public bool IsMonitoring
		{
			get { return isMonitoring; }
		}

		//Servers name
		private string serverName;
		public string ServerName
		{
			get { return serverName; }
		}

		//Servers log file name
		private string logFilePath;
		public string LogFilePath
		{
			get { return logFilePath; }
		}

		public ServerLogMonitor(string serverName, string logFilePath)
		{
			this.serverName = serverName;
			this.logFilePath = logFilePath;
		}

		public void Start()
		{
			if (!isMonitoring)
			{
				//Open logs
				logFileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				logFileReader = new StreamReader(logFileStream);

				//Set monitoring status
				isMonitoring = true;

				//Start the monitor task
				monitor = Task.Run(() => Monitor());
			}
		}

		public void Stop()
		{
			isMonitoring = false;
			// Clean up
			logFileReader.Close();
			logFileStream.Close();
		}

		private void Monitor()
		{
			while (isMonitoring)
			{
				//Make sure there are new lines to read
				if (!logFileReader.EndOfStream)
				{
					string line = logFileReader.ReadLine();
					
					//Check if server starting
					if (line.Contains("Application start"))
					{
						//Special case for proxy server
						if (serverName == "Proxy")
						{
							ServerStatusManager.Instance.ChangeServerStatus(serverName, ServerStatus.Online);
						}
						else
						{
							ServerStatusManager.Instance.ChangeServerStatus(serverName, ServerStatus.Starting);
						}
					}
					//Check if server stopped
					else if (line.Contains("Application stop"))
					{
						ServerStatusManager.Instance.ChangeServerStatus(serverName, ServerStatus.Offline);
					}
					//Check if server is running
					else if (line.Contains("Registering sub server"))
					{
						ServerStatusManager.Instance.ChangeServerStatus(serverName, ServerStatus.Online);
					}


				}
				else
				{
					//If at end of file, sleep for 500ms
					Thread.Sleep(500);
				}
			}
		}
	}
}