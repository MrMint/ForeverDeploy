using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	[Table("Deployments")]
	public class Deployment
	{
		[Key]
		[JsonIgnore]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[JsonIgnore]
		public DateTime DateDeployedUTC { get; set; }

		//Commit foreign key
		[JsonIgnore]
		public int CommitId { get; set; }
		
		//Lazy loaded commit object
		[JsonProperty("commit")]
		public virtual DeployedCommit Commit { get; set; }

		//Current status of the deployment
		[JsonProperty("deploymentStatus")]
		public DeploymentStatus DeploymentStatus { get; set; }

		//Returns the UTC date of the deployment in ticks
		[JsonProperty("dateDeployedUTCTicks")]
		public long DateDeployedUTCTicks
		{
			get
			{
				return DateDeployedUTC.Ticks;
			}
		}

		//Number of build warnings
		[JsonProperty("buildWarnings")]
		public int BuildWarnings { get; set; }

		//Number of build errors
		[JsonProperty("buildErrors")]
		public int BuildErrors { get; set; }

		//Estimated percent complete of deployment
		[NotMapped]
		[JsonProperty("percentComplete")]
		public int PercentComplete { get; set; }
	}


}