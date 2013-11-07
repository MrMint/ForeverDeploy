using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	//Enum representing a servers current status
	public enum ServerStatus : byte
	{
		Offline = 0,
		Starting = 1,
		Running = 2
	}
}