using ForeverDeploy.Models;
using ForeverDeploy.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForeverDeploy.Controllers
{
	public class HomeController : Controller
	{
		//Logger
		private static Logger log = LogManager.GetCurrentClassLogger();

		public ActionResult Index()
		{
			return View();
		}

		public void RunTestCase()
		{
			log.Debug("Running deploy test");
			try
			{
				//Process the new commits
				var commits = new List<Commit>()
				{
					new Commit()
					{
						message = "This is a commit",
						branch = "features"
					},
					new Commit()
					{
						message = "This is a test commit #deploy",
						branch = "master",
						raw_node = "aadv31456134613461345yqefyqey34",
						utctimestamp = DateTime.UtcNow.ToString()
					}
				};
				DeploymentManager.Instance.DeployNewCommits(commits);
			}
			catch (Exception e)
			{
				log.Error("Error in deploy test: {0}, Trace: {1}", e.Message, e.StackTrace);
			}
		}
	}
}
