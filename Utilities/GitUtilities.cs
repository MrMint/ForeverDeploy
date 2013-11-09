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
using NGit;
using System.IO;

namespace ForeverDeploy.Utilities
{
	public class GitUtilities
	{
		private static Logger log = LogManager.GetCurrentClassLogger();

		private static List<string> deploymentKeywords = new List<string>()
		{
			"#yolo",
			"#deploy"
		};

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
		public static Commit ProcessNewCommits(List<ForeverDeploy.Models.Commit> commits)
		{
			DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.ProcessingCommits;
			log.Debug("Processing {0} commits", commits.Count);
			//Parse commits looking for a commit on master or one with deployment keyword
			return commits.Where(x =>

				//Get commits that are from master branch
				String.Equals(x.branch, "master") ||

				//Get commits that contain a deployment keyword
				deploymentKeywords.Any<string>(s => Regex.Match(x.message, s, RegexOptions.IgnoreCase).Success))

				//Order those by most recent
				.OrderByDescending(x => x.utctimestamp)

				//Get first
				.FirstOrDefault();
		}

		/// <summary>
		/// Updates the repository given a commit
		/// </summary>
		/// <param name="commit">Repository was updated</param>
		/// <returns></returns>
		public static bool UpdateRepository(DeployedCommit commit)
		{
			//TODO: ProgressMonitor does not work, investigate
			DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.OpeningRepo;
			try
			{

				log.Debug("Setting up GIT ssh");
				// Use our custom SSH session when accessing remote SSH:// repositories
				// The username must be in the repository Uri: ssh://git@host/var/git/repo.git
				var factory = new PrivateKeyConfigSessionFactory(FDConfig.Instance.PrivateKeyPath);
				SshSessionFactory.SetInstance(factory);

				//Open the repository
				log.Debug("Opening the local repo");
				var repository = Git.Open(FDConfig.Instance.LocalRepositoryPath);

				//Get the list of local branches
				log.Debug("Getting list of local branches");
				var refList = repository.BranchList().SetListMode(ListBranchCommand.ListMode.HEAD).Call();

				DeploymentManager.Instance.DeploymentStatus = DeploymentStatus.UpdatingRepo;
				using (var streamWriter = new StreamWriter(FDConfig.Instance.GitLogPath))
				{
					var tPM = new TextProgressMonitor(streamWriter);

					//Check if local repo already has the branch
					if (refList.Any(x => Regex.Match(x.GetName(), commit.Branch).Success))
					{
						//Branch exists, check it out forcefully
						log.Debug("Branch {0} already exists locally, checking out", commit.Branch);
						repository.Checkout()
							.SetName(commit.Branch)
							.SetForce(true)
							.Call();

						//Pull new changes
						log.Debug("Pulling new changes");
						var result = repository.Pull()
							.SetProgressMonitor(tPM)
							.Call();

						log.Debug("Merge Status: " + result.GetMergeResult().GetMergeStatus());
						var mergeStatus = result.GetMergeResult().GetMergeStatus();

						//If the merge fails, do a hard reset
						if (mergeStatus == MergeStatus.FAILED)
						{
							log.Debug("Merge failed, hard reseting");
							repository.Reset()
								.SetMode(ResetCommand.ResetType.HARD)
								.SetRef("origin/" + commit.Branch)
								.Call();
							return true;
						}
						return result.IsSuccessful();
					}
					else
					{
						//Add refs
						var config = repository.GetRepository().GetConfig();
						config.SetString("branch", commit.Branch, "remote", "origin");
						config.SetString("branch", commit.Branch, "merge", "refs/heads/" + commit.Branch);
						config.Save();

						//Branch does not exist, create and check it out
						log.Debug("Branch {0} does not exist locally, creating and checking out from origin", commit.Branch);

						//Do a checkout and create the branch at the same time
						repository.Checkout()
							.SetCreateBranch(true)
							.SetName(commit.Branch)
							.Call();

						//Pull new changes
						log.Debug("Pulling new changes");
						var result = repository.Pull()
							.SetProgressMonitor(tPM)
							.Call();

						log.Debug("Merge status: " + result.GetMergeResult().GetMergeStatus());
						return result.IsSuccessful();
					}
				}
			}
			catch (Exception e)
			{
				return false;
				log.Error("Exception in UpdateRepository(): {0}, Trace: {1}, InnerException: {2}", e.Message, e.StackTrace, e.InnerException);
			}

		}
	}
}