using ForeverDeploy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using NGit.Api;
using System.Collections.ObjectModel;
using NGit.Transport;
using NLog;

namespace ForeverDeploy.Utilities
{
	public class GitUtilities
	{
		private static Logger log = LogManager.GetCurrentClassLogger();
		//TODO: Not thread safe homies
		private static List<string> deploymentKeywords = new List<string>()
		{
			"#yolo",
			"#deploy"
		};

		private static string localRepositoryPath = @"D:\ForeverDeploy\voxelscape";
		//private static string localRepositoryPath = @"E:\workspace\voxelscape";
		private static string remoteRepositoryURI = "git@bitbucket.org:Mr_Mint/voxelscape.git";

		//Public property for deployment keywords
		public static List<string> DeploymentKeywords
		{
			get
			{
				return deploymentKeywords;
			}
			set
			{
				deploymentKeywords = value;
			}
		}

		/// <summary>
		/// Utility function that figures out what needs to be done when new commits are received
		/// </summary>
		/// <param name="commits">Commits received</param>
		public static void ProcessNewCommits(List<ForeverDeploy.Models.Commit> commits)
		{
			log.Debug("Processing {0} commits", commits.Count);
			//Parse commits looking for a commit on master or one with deployment keyword
			var deploymentCommit = commits.Where(x =>

				//Get commits that are from master branch
				String.Equals(x.branch, "master") ||

				//Get commits that contain a deployment keyword
				deploymentKeywords.Any<string>(s => Regex.Match(x.message, s, RegexOptions.IgnoreCase).Success))

				//Order those by most recent
				.OrderByDescending(x => x.utctimestamp)

				//Get first
				.FirstOrDefault();

			if (deploymentCommit != null)
			{
				log.Debug("Found a deployable commit, Branch: {0}, Author: {1}, Message: {2}", deploymentCommit.branch, deploymentCommit.author, deploymentCommit.message);
				//If valid commit found, pass it along
				UpdateRepository(deploymentCommit);
			}
		}

		public static void UpdateRepository(ForeverDeploy.Models.Commit commit)
		{
			try
			{

				log.Debug("Setting up GIT ssh");
				// Use our custom SSH session when accessing remote SSH:// repositories
				// The username must be in the repository Uri: ssh://git@host/var/git/repo.git
				var privateKeyPath = @"C:\Users\Administrator\.ssh\id_rsa";
				//var privateKeyPath = @"E:\workspace\ForeverDeploy\keys";
				var factory = new PrivateKeyConfigSessionFactory(privateKeyPath);
				SshSessionFactory.SetInstance(factory);

				//Open the repository
				log.Debug("Opening the local repo");
				var repository = Git.Open(localRepositoryPath);

				//Get the list of local branches
				log.Debug("Getting list of local branches");
				var refList = repository.BranchList().SetListMode(ListBranchCommand.ListMode.HEAD).Call();

				//Check if local repo already has the branch
				if (refList.Any(x => Regex.Match(x.GetName(), commit.branch).Success))
				{
					//Branch exists, check it out forcefully
					log.Debug("Branch {0} already exists locally, checking out", commit.branch);
					repository.Checkout()
						.SetName("refs/head/" + commit.branch)
						.SetForce(true)
						.Call();

					//Pull new changes
					log.Debug("Pulling new changes");
					repository.Pull()
						.Call();
				}
				else
				{
					//Add refs
					var config = repository.GetRepository().GetConfig();
					config.SetString("branch", commit.branch, "remote", "origin");
					config.SetString("branch", commit.branch, "merge", "refs/heads/" + commit.branch);
					config.Save();
					
					//Branch does not exist, create and check it out
					log.Debug("Branch {0} does not exist locally, creating and checking out from origin", commit.branch);

					//Do a checkout and create the branch at the same time
					repository.Checkout()
						.SetCreateBranch(true)
						.SetName(commit.branch)
						.Call();

					//Pull new changes
					log.Debug("Pulling new changes");
					repository.Pull()
						.Call();


				}
			}
			catch (Exception e)
			{
				log.Error("Exception in UpdateRepository(): {0}, Trace: {1}, InnerException: {2}", e.Message, e.StackTrace, e.InnerException);
			}

		}
	}
}