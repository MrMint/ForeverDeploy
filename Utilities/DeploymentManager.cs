using ForeverDeploy.Hubs;
using ForeverDeploy.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Utilities
{
	public sealed class DeploymentManager
	{
		//Singleton related variables
		private static volatile DeploymentManager instance;
		private static object syncRoot = new Object();
		private static object syncDeploy = new Object();

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		//Current Deployment
		private Deployment deployment;

		private DeploymentManager()
		{
			var db = new FDContext();
			deployment = db.Deployments.OrderByDescending(x => x.DateDeployedUTC).FirstOrDefault();
		}

		//Get singleton instance
		public static DeploymentManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new DeploymentManager();
					}
				}

				return instance;
			}
		}

		//Property for setting the current deployments status
		public DeploymentStatus DeploymentStatus
		{
			get
			{
				return deployment.DeploymentStatus;
			}
			set
			{
				deployment.DeploymentStatus = value;
				UpdateClients();
			}
		}

		//Property for the current deployment percent complete
		public int PercentComplete
		{
			get
			{
				return deployment.PercentComplete;
			}
			set
			{
				deployment.PercentComplete = value;
				log.Debug("DEPLOYMENT: {0}% Complete", value);
				//Notify clients via websockets of new build status info
				UpdateClients();
			}
		}

		//Property for the current builds error count
		public int BuildErrorCount
		{
			get
			{
				return deployment.BuildErrors;
			}
			set
			{
				deployment.BuildErrors = value;
				log.Debug("BUILD: {0} Error(s)", value);
				//Notify clients via websockets of new build status info
				UpdateClients();
			}
		}

		//Property for the current builds warning count
		public int BuildWarningCount
		{
			get
			{
				return deployment.BuildWarnings;
			}
			set
			{
				deployment.BuildWarnings = value;
				log.Debug("BUILD: {0} Warnings(s)", value);
				//Notify clients via websockets of new build status info
				UpdateClients();
			}
		}

		/// <summary>
		/// Creates and deploys new deployments
		/// </summary>
		/// <param name="commits">The new commits to be parsed</param>
		public void DeployNewCommits(List<Commit> commits)
		{
			lock (syncDeploy)
			{
				deployment = new Deployment();

				//Process the new commits
				deployment.Commit = new DeployedCommit(GitUtilities.ProcessNewCommits(commits));

				if (deployment.Commit != null)
				{
					//Log debug message
					log.Debug("Found a deployable commit, Branch: {0}, Author: {1}, Message: {2}",
						deployment.Commit.Branch,
						deployment.Commit.Author,
						deployment.Commit.Message
						);

					//Valid commit found, update repository
					var updated = GitUtilities.UpdateRepository(deployment.Commit);

					if (updated)
					{
						//Repo was updated, time to build!
						log.Debug("BUILD: Compiling");
						DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.Building;

						var compiled = DeploymentUtilities.Build(deployment);
						if (compiled)
						{
							log.Debug("BUILD: Successfully compiled");
							DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.Deployed;
						}
						else
						{
							log.Error("BUILD: Failed to compile");
							DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.BuildingFailed;
						}
					}
					else
					{
						log.Error("GIT: Failed to update.");
						DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.UpdatingRepoFailed;
					}
				}
				else
				{
					log.Debug("POST: No deployable commits found.");
				}
			}
		}

		//Helper method for updating connect clients on deployment status
		public void UpdateClients()
		{
			Broadcaster.Instance.UpdateClients(deployment);
		}

	}

}