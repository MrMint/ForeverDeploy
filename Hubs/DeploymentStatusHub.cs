using ForeverDeploy.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Hubs
{
	[HubName("deploymentStatusHub")]
	public class DeploymentStatusHub : Hub
	{
		public void Connect()
		{
			Broadcaster.Instance.SendOldDeployments(Context.ConnectionId);
			Broadcaster.Instance.SendServerStatus(Context.ConnectionId);
		}

		public void UpdateStatus()
		{

		}
	}
}