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
		private List<Server> servers { get; set; }
		public List<Server> Servers
		{
			get
			{
				return servers;
			}
		}

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		private ServerStatusManager()
		{
			//TODO: Automatically detect servers based on log files present

			servers = new List<Server>();

			//Add Proxy server
			servers.Add(new Server()
			{
				Name = "Proxy",
				LogFileName = "",
				ServerStatus = ServerStatus.Checking
			});

			//Add Login server
			servers.Add(new Server()
			{
				Name = "Login",
				LogFileName = "",
				ServerStatus = ServerStatus.Checking
			});

			//Add Game server
			servers.Add(new Server()
			{
				Name = "Game",
				LogFileName = "",
				ServerStatus = ServerStatus.Checking
			});
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


	}
}