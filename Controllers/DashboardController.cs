using ForeverDeploy.Extensions;
using ForeverDeploy.Models;
using ForeverDeploy.Utilities;
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
			return View("Layout");
		}

		//
		// GET: /Dashboard/Dashboard

		public ActionResult Dashboard()
		{
			return PartialView("DashboardTemplate");
		}

		//
		// GET: /Dashboard/BuildLog

		public string BuildLog(string Id)
		{
			//Return request buildlog with proper formatting
			return BuildLogUtilities.GetBuildLog(Id);
		}
	}
}
