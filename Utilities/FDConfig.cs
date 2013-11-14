using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Utilities
{
	public sealed class FDConfig
	{
		//Singleton related variables
		private static volatile FDConfig instance;
		private static object syncRoot = new Object();

		//Builder variables
		public string Builder { get; set; }
		public string BuilderWorkingDirectory { get; set; }
		public string BuildLogsPath { get; set; }
		public string BuildTargetPath { get; set; }

		//Repository variables
		public string LocalRepositoryPath { get; set; }
		public string RemoteRepositoryURI { get; set; }
		public string GitLogPath { get; set; }

		//Private key variables
		public string PrivateKeyPath { get; set; }
		public string PrivateKeyPassword { get; set; }
		
		//POST Token
		public string PostToken { get; set; }

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		private FDConfig() { }

		//Get singleton instance
		public static FDConfig Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new FDConfig();
					}
				}

				return instance;
			}
		}
	}
}