using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	public class Server
	{
		//Name of the server
		[JsonProperty("name")]
		public string Name { get; set; }

		//Servers log
		[JsonProperty("logFileName")]
		public string LogFileName { get; set; }

		//Servers status
		[JsonProperty("serverStatus")]
		public ServerStatus ServerStatus { get; set; }
	}
}