using ForeverDeploy.Hubs;
using ForeverDeploy.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Utilities
{
	public class ServerStatusManager
	{
		//Singleton related variables
		private static volatile ServerStatusManager instance;
		private static object syncRoot = new Object();

		//List of current servers
		private List<Server> servers { get; set; }

		public List<Server> Servers
		{
			get
			{
				return servers;
			}
		}

		private List<ServerLogMonitor> monitors;

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		private ServerStatusManager()
		{
			//TODO: Automatically detect servers based on log files present
			
			//Initialize server objects
			InitializeServers();

			//Initialize server monitors
			InitializeMonitors();
		}

		//Get singleton instance
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
				log.Error("ServerStatus: Unable to find requested server {0}, to update with status {1}",serverName,status);
			}
		}

		//Starts all server monitors
		public void StartServerMonitors()
		{
			foreach (var monitor in monitors)
			{
				monitor.Start();
			}
		}

		//Stops all server monitors
		public void StopServerMonitors()
		{
			foreach (var monitor in monitors)
			{
				monitor.Stop();
			}
		}

		//Helper function for initializing servers
		private void InitializeServers()
		{
			servers = new List<Server>();

			//Add Proxy server
			servers.Add(new Server()
			{
				Name = "Proxy",
				LogFileName = "Proxy",
				ServerStatus = ServerStatus.Checking
			});

			//Add Login server
			servers.Add(new Server()
			{
				Name = "Login",
				LogFileName = "Login",
				ServerStatus = ServerStatus.Checking
			});

			//Add Game server
			servers.Add(new Server()
			{
				Name = "Game",
				LogFileName = "Game",
				ServerStatus = ServerStatus.Checking
			});
		}

		//Helper function for initializing monitors
		private void InitializeMonitors()
		{
			monitors = new List<ServerLogMonitor>();
			
			foreach (var server in servers)
			{
				monitors.Add(new ServerLogMonitor(server.Name, server.LogFileName));
			}
		}
	}
}