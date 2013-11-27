using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ForeverDeploy.Models;
using ForeverDeploy.Utilities;
using NLog;
using ForeverDeploy.Extensions;

namespace ForeverDeploy.Controllers
{
	public class GitHooksController : Controller
	{
		private static Logger log = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// POST hook for BitBucket
		/// </summary>
		/// <param name="key">The key to verify sender.</param>
		[HttpPost, ValidateInput(false)]
		public void Bitbucket(string key)
		{
			//Verify given token
			if (key == FDConfig.Instance.PostToken)
			{
				log.Debug("POST from BitBucket received, Form null: {0}, Contains payload key: {1}", Request.Form == null, Request.Form.AllKeys.Contains("payload"));
				RootObject payload;
				try
				{
					//Get payload from POST and deserialize
					payload = JsonConvert.DeserializeObject<RootObject>(Request.Form["payload"]);
				}
				catch (Exception e)
				{
					//Something went wrong deserializing
					log.Error("Error de-serializing POST payload");
					log.LogExceptionExt(e);
					return;
				}

				if (payload.commits != null)
				{
					log.Debug("Payload de-serialized, {0} commits received", payload.commits.Count);

					//Pass to deployment manager for deployment
					DeploymentManager.Instance.DeployNewCommits(payload.commits);
				}
				else
				{
					log.Error("Payload de-serialized, payload.commits is null!");
				}


			}
		}
	}
}
