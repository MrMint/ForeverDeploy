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
			return View("Layout");
		}
	}
}
