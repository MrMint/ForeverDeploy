using ForeverDeploy.Models;
using Microsoft.Build.Evaluation;
//using Microsoft.Build.Execution;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ForeverDeploy.Utilities
{
	public static class DeploymentUtilities
	{
		private static Logger log = LogManager.GetCurrentClassLogger();

		internal static bool Build(Deployment deployment)
		{
			var logName = deployment.Commit.RawNode;

			// Fix to the path of your msbuild
			var pathToMsBuild = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\MSBuild.exe";
			var pathToSolution = @"D:\ForeverDeploy\voxelscape\Voxelscape.sln";
			var logger = String.Format("/fl1 /flp1:FileLogger,Microsoft.Build.Engine;logfile=D:/ForeverDeploy/logs/build/{0}.txt;verbosity=normal;append=false;encoding=utf-8;performancesummary", logName);

			//Create and setup process
			var builder = new Process();
			builder.StartInfo.WorkingDirectory = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319";
			builder.StartInfo.FileName = "MSBuild.exe";
			builder.StartInfo.Arguments = " " + pathToSolution + " " + logger;

			//Start process
			builder.Start();

			Thread.Sleep(1000);

			//Open build logs
			FileStream logFileStream = new FileStream(String.Format("D:/ForeverDeploy/logs/build/{0}.txt", logName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			StreamReader logFileReader = new StreamReader(logFileStream);

			
			bool done = false;
			var complete = false;
			var built = false;
			var infiniteLoopProtector = 0;
			
			//Monitor compile progress
			while (!done)
			{
				//Make sure there are new lines to read
				if (!logFileReader.EndOfStream)
				{
					string line = logFileReader.ReadLine();

					//Build failed
					if (line.Contains("Build FAILED."))
					{
						built = false;
					}

					//Build succeeded
					if (line.Contains("Build succeeded."))
					{
						built = true;
					}

					//Get number of errors
					if (line.Contains("Error(s)"))
					{
						if (Convert.ToInt32(line.Replace(" ", "").Substring(0, 1)) > 0)
						{
							DeploymentManager.Instance.BuildErrorCount = Convert.ToInt32(line.Replace(" ", "").Substring(0, 1));
						}
					}

					//Get number of warnings
					if (line.Contains("Warning(s)"))
					{
						//Remove white spaces
						var warn = line.Replace(" ", "");
						if (Convert.ToInt32(warn.Substring(0, 1)) > 0)
						{

							DeploymentManager.Instance.BuildWarningCount = Convert.ToInt32(warn.Substring(0, warn.IndexOf('W')));
						}
					}

					//Reached end of log
					if (line.Contains("Time Elapsed"))
					{
						//log.Debug("BUILD: Successful! {0}",line);
						done = true;
					}
				}
				else
				{
					//If at end of file, sleep for 150ms
					Thread.Sleep(150);
				}

				//Protect from an infinite loop
				if (infiniteLoopProtector > 25000)
				{
					done = true;
					built = false;
				}
				infiniteLoopProtector++;
			}

			// Clean up
			logFileReader.Close();
			logFileStream.Close();

			return built;
		}
	}
}