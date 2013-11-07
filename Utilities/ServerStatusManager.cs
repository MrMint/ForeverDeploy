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

		//Deployment lock
		private static object syncDeploy = new Object();

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		private ServerStatusManager()
		{

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