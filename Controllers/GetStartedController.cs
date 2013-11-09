using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForeverDeploy.Controllers
{
    public class GetStartedController : Controller
    {
        //
        // GET: /GetStarted/

        public ActionResult Index()
        {
            return View("Layout");
        }

		//
		// GET: /GetStarted/

		public ActionResult GetStarted()
		{
			return PartialView("GetStarted");
		}
    }
}
