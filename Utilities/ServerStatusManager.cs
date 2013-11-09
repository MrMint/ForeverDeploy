using ForeverDeploy.Hubs;
using ForeverDeploy.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Utilities
{
	public sealed class ServerStatusManager
	{
		//Singleton related variables
		private static volatile ServerStatusManager instance;
		private static object syncRoot = new Object();

		//List of current servers
		private List<Server> servers = new List<Server>();

		public List<Server> Servers
		{
			get
			{
				return servers;
			}
		}

		private List<ServerLogMonitor> monitors = new List<ServerLogMonitor>();

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		private ServerStatusManager() { }

		/// <summary>
		/// Get singleton instance
		/// </summary>
		public static ServerStatusManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new ServerStatusManager();
					}
				}

				return instance;
			}
		}

		/// <summary>
		/// Changes the given servers status, and alerts clients to the change via websockets
		/// </summary>
		/// <param name="serverName">Name of the server</param>
		/// <param name="status">Servers status</param>
		public void ChangeServerStatus(string serverName, ServerStatus status)
		{
			var server = servers.FirstOrDefault(x => x.Name == serverName);
			if (server != null)
			{
				server.ServerStatus = status;
				Broadcaster.Instance.UpdateClientsServerStatus(server);
				log.Debug("ServerStatus: Server {0}: {1}", serverName, status);
			}
			else
			{
				log.Error("ServerStatus: Unable to find requested server {0}, to update with status {1}", serverName, status);
			}
		}

		/// <summary>
		/// Starts all server monitors
		/// </summary>
		public void StartServerMonitors()
		{
			foreach (var monitor in monitors)
			{
				monitor.Start();
			}
		}

		/// <summary>
		/// Stops all server monitors
		/// </summary>
		public void StopServerMonitors()
		{
			foreach (var monitor in monitors)
			{
				monitor.Stop();
			}
		}

		/// <summary>
		/// Adds a server to the list of servers
		/// </summary>
		/// <param name="server">Server to be added.</param>
		public void AddServer(Server server)
		{
			servers.Add(server);
			InitializeMonitor(server);
		}

		/// <summary>
		/// Helper function for initializing a monitor for a given server
		/// </summary>
		/// <param name="server">Server to monitor.</param>
		private void InitializeMonitor(Server server)
		{
			monitors.Add(new ServerLogMonitor(server.Name, server.LogFilePath));
		}
	}
}