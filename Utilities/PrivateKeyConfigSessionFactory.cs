using NGit.Transport;
using NGit.Util;
using NLog;
using NSch;
using Sharpen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Utilities
{
	/// <summary>
	/// Handles setting up the public key authentication when using a remote SSH repository
	/// </summary>
	public class PrivateKeyConfigSessionFactory : JschConfigSessionFactory
	{
		private string PrivateKeyPath { get; set; }

		private static NLog.Logger log = LogManager.GetCurrentClassLogger();

		public PrivateKeyConfigSessionFactory(string privateKeyPath)
		{
			PrivateKeyPath = privateKeyPath;

			// Clear the GIT_SSH environment variable as NGit will use it for SSH transport instead of the session factory
			Environment.SetEnvironmentVariable("GIT_SSH", string.Empty, EnvironmentVariableTarget.Process);
		}

		protected override void Configure(OpenSshConfig.Host hc, Session session)
		{
			try
			{
				var config = new Properties();
				config["StrictHostKeyChecking"] = "no";
				config["PreferredAuthentications"] = "publickey";
				session.SetConfig(config);

				var jsch = GetJSch(hc, FS.DETECTED);
				var passphrase = System.Text.Encoding.ASCII.GetBytes(FDConfig.Instance.PrivateKeyPassword);
				jsch.AddIdentity("KeyPair", File.ReadAllBytes(PrivateKeyPath), null, passphrase);
			}
			catch (Exception e)
			{
				log.Error("Exception in PrivateKeyConfigSessionFactory: {0}", e.Message);
				throw new Exception("Error in PrivateKeyConfigSessionFactory");
			}
		}
	}
}