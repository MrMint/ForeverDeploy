using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	//Enum representing a servers current status
	public enum ServerStatus : byte
	{
		Checking = 0,
		Offline = 1,
		Starting = 2,
		Online = 3
	}
}