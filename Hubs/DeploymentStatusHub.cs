using ForeverDeploy.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Hubs
{
	/// <summary>
	/// The websocket hub used by the clients
	/// </summary>
	[HubName("deploymentStatusHub")]
	public class DeploymentStatusHub : Hub
	{
		public void Connect()
		{
			Broadcaster.Instance.SendOldDeployments(Context.ConnectionId);
			Broadcaster.Instance.SendServerStatus(Context.ConnectionId);
		}
	}
}