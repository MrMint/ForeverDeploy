using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace ForeverDeploy.Utilities
{
	public static class BuildLogUtilities
	{

		public static string GetBuildLog(string fileName)
		{

			//Open build logs
			FileStream logFileStream = new FileStream(String.Format("D:/ForeverDeploy/logs/build/{0}.txt", fileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			StreamReader logFileReader = new StreamReader(logFileStream);
			var log = logFileReader.ReadToEnd();
			log = log.Replace(" ", "&nbsp;");
			log = log.Replace(System.Environment.NewLine, "<br />");
			
			return log;
		}
	}
}