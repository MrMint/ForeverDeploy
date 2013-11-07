using ForeverDeploy.Hubs;
using ForeverDeploy.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForeverDeploy.Extensions;
using System.Threading;

namespace ForeverDeploy.Utilities
{
	public sealed class DeploymentManager
	{
		//Singleton related variables
		private static volatile DeploymentManager instance;
		private static object syncRoot = new Object();

		//Deployment lock
		private static object syncDeploy = new Object();

		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		//Current Deployment
		private Deployment deployment;

		public Deployment Deployment
		{
			get
			{
				return deployment;
			}
		}
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
				var failed = false;
				var updated = false;
				deployment = new Deployment();

				//Process the new commits
				var commit = GitUtilities.ProcessNewCommits(commits);

				if (commit != null)
				{
					deployment.Commit = new DeployedCommit(commit);
					deployment.DatePreparedUTC = DateTime.UtcNow;
					UpdateClients();
					//Log debug message
					log.Debug("Found a deployable commit, Branch: {0}, Author: {1}, Message: {2}",
						deployment.Commit.Branch,
						deployment.Commit.Author,
						deployment.Commit.Message
						);

					try
					{
						//updated = true;
						//Thread.Sleep(2000);

						//Valid commit found, update repository
						updated = GitUtilities.UpdateRepository(deployment.Commit);
						deployment.DateUpdatedUTC = DateTime.UtcNow;
					

					}
					catch (Exception e)
					{
						log.Error("GIT: Failed to update due to exception.");
						log.LogExceptionExt(e);
						deployment.DateUpdatedUTC = DateTime.UtcNow;
						DeploymentStatus = DeploymentStatus.UpdatingRepoFailed;
						failed = true;
					}

					if (!failed)
					{
						var compiled = false;
						try
						{
							//Repo was updated, time to build!
							log.Debug("BUILD: Compiling");
							DeploymentStatus = DeploymentStatus.Building;
							
							//Thread.Sleep(3000);
							//compiled = true;

							compiled = DeploymentUtilities.Build(deployment);
							deployment.DateBuiltUTC = DateTime.UtcNow;
						}
						catch (Exception e)
						{
							log.Error("BUILD: Failed to build due to exception.");
							log.LogExceptionExt(e);
							deployment.DateBuiltUTC = DateTime.UtcNow;
							DeploymentStatus = DeploymentStatus.BuildingFailed;
							failed = true;
						}

						if (compiled && !failed)
						{
							log.Debug("BUILD: Successfully compiled");
							Deployment.DateDeployedUTC = DateTime.UtcNow;
							DeploymentStatus = DeploymentStatus.Deployed;

						}
						else if (!compiled)
						{
							log.Error("BUILD: Failed to due to errors.");
							Deployment.DateDeployedUTC = DateTime.UtcNow;
							DeploymentStatus = DeploymentStatus.BuildingFailed;
						}
					}
				}
				else
				{
					log.Debug("No deployable commits found");
				}

				FDContext db = new FDContext();
				db.Deployments.Add(deployment);
				db.SaveChanges();
			}
		}

		//Helper method for updating connect clients on deployment status
		public void UpdateClients()
		{
			Broadcaster.Instance.UpdateClients(deployment);
		}

	}

}