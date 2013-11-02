using ForeverDeploy.Extensions;
using ForeverDeploy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForeverDeploy.Controllers
{
	public class DashboardController : Controller
	{
		//
		// GET: /Dashboard/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Dashboard()
		{
			return PartialView("Dashboard");
		}

		public ActionResult DeploymentTemplate()
		{
			return PartialView("_DeploymentTemplatePartial");
		}

		public JsonDotNetResult DeploymentItemTest()
		{
			var commit = new Commit()
			{
				author = "Mr_Mint",
				branch = "master",
				message = "This is a message",
				raw_author = "Jared Prather <pratherjared@gmail.com>",
				raw_node = "134u56h134i6ub134i56u3b",
				utctimestamp = DateTime.UtcNow.ToString()
			};

			return new JsonDotNetResult(new List<Deployment>()
			{
				new Deployment()
			{
				Commit = new DeployedCommit()
				{
					Author = "Jared Prather",
					Branch = "master",
					UtcTimeStamp = DateTime.UtcNow.ToString(),
					Message = "Testing new deployment methods"
				},
				DeploymentStatus = DeploymentStatus.POSTReceived,
				PercentComplete = 55
			}
			});
		}
	}
}
