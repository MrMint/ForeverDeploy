using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	public class Server
	{
		//Name of the server
		public string Name { get; set; }

		//Servers log
		public string LogFileName { get; set; }

		//Servers status
		public ServerStatus ServerStatus { get; set; }
	}
}