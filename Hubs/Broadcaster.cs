using ForeverDeploy.Models;
using ForeverDeploy.Utilities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Hubs
{
	/// <summary>
	/// The singleton that manages broadcasting websocket messages to clients
	/// </summary>
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

		/// <summary>
		/// Sends a deployment to clients
		/// </summary>
		/// <param name="deployment">The deployment to be sent.</param>
		/// <param name="connectionId">The connectionId to send the deployment to.(optional)</param>
		/// <param name="connectionIds">The list connectionIds to send the deployment to.(optional)</param>
		public void UpdateClients(Deployment deployment, string connectionId = null, List<string> connectionIds = null)
		{
			if (connectionId == null && connectionIds == null)
			{
				deploymentStatusHubContext.Clients.All.updateDeployment(deployment);
			}
			else if(connectionId != null)
			{
				deploymentStatusHubContext.Clients.Client(connectionId).updateDeployment(deployment);
			}
		}

		public void SendOldDeployments(string connectionId)
		{
			FDContext db = new FDContext();
			deploymentStatusHubContext.Clients.Client(connectionId).oldDeployments(db.Deployments.Include("Commit").OrderByDescending(x=>x.DateDeployedUTC).Take(3));
		}

		/// <summary>
		/// Send current servers to the client
		/// </summary>
		/// <param name="connectionId">ConnectionId to send to.</param>
		public void SendServerStatus(string connectionId)
		{
			deploymentStatusHubContext.Clients.Client(connectionId).servers(ServerStatusManager.Instance.Servers);
		}

		public void UpdateClientsServerStatus(Server server)
		{
			deploymentStatusHubContext.Clients.All.updateServer(server);
		}
	}
}