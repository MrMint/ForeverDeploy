using ForeverDeploy.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Hubs
{
	public class Broadcaster
	{
		private readonly static Lazy<Broadcaster> instance = new Lazy<Broadcaster>(() => new Broadcaster());

		private readonly IHubContext deploymentStatusHubContext;

		public static Broadcaster Instance
		{
			get
			{
				return instance.Value;
			}
		}

		private Broadcaster()
		{
			deploymentStatusHubContext = GlobalHost.ConnectionManager.GetHubContext<DeploymentStatusHub>();
		}

		public void UpdateClients(Deployment deployment)
		{
			deploymentStatusHubContext.Clients.All.updateDeployment(deployment);
		}
	}
}