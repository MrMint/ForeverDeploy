using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace ForeverDeploy.Extensions
{
	public class MustBeAJAX : IRouteConstraint
	{
		public MustBeAJAX() { }

		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
				if(values.ContainsKey("controller") && values.ContainsKey("action"))
				{
					if(values["controller"] == "Home" && values["action"] == "Index")
					{
						return true;
					}
				}
			return httpContext.Request.IsAjaxRequest();
		}
	}
}
