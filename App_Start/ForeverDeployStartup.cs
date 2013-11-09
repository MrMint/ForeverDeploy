using ForeverDeploy.Models;
using ForeverDeploy.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using ForeverDeploy.Extensions;

namespace ForeverDeploy
{
	public static class ForeverDeployStartup
	{
		private static Logger log = LogManager.GetCurrentClassLogger();

		public static void StartUp()
		{
			try
			{
				//Load config
				var configFile = new XmlDocument();
				configFile.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Voxelscape.config"));
				var config = configFile["configuration"];

				//Load Paths
				FDConfig.Instance.LocalRepositoryPath = config["localRepositoryPath"].InnerText;
				FDConfig.Instance.BuildTargetPath = config["buildTargetPath"].InnerText;
				FDConfig.Instance.PrivateKeyPath = config["privateKeyPath"].InnerText;
				FDConfig.Instance.GitLogPath = config["gitLogPath"].InnerText;
				FDConfig.Instance.Builder = config["builder"].InnerText;
				FDConfig.Instance.BuilderWorkingDirectory = config["builderWorkingDirectory"].InnerText;
				FDConfig.Instance.RemoteRepositoryURI = config["remoteRepositoryURI"].InnerText;
				FDConfig.Instance.BuildLogsPath = config["buildLogsPath"].InnerText;

				//Load password
				FDConfig.Instance.PrivateKeyPassword = config["privateKeyPassword"].InnerText;

				//Load servers
				foreach (XmlNode element in config["servers"].ChildNodes)
				{
					if (element.HasChildNodes)
					{
						ServerStatusManager.Instance.AddServer(new Server()
						{
							Name = element["serverName"].InnerText,
							LogFilePath = element["serverLogPath"].InnerText,
							ServerStatus = ServerStatus.Checking
						});
					}
				}
			}
			catch (Exception e)
			{
				log.LogExceptionExt(e);
				throw new Exception("Error while attempting to load configuration file!");
			}
			//Start server monitors
			ServerStatusManager.Instance.StartServerMonitors();

		}
	}
}