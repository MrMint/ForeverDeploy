using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ForeverDeploy.Models
{
	//Model representing a deployment
	[Table("Deployments")]
	public class Deployment
	{
		[Key]
		[JsonIgnore]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		//Date the deployment was prepared
		[JsonIgnore]
		public DateTime DatePreparedUTC { get; set; }

		//Date the deployment was updated
		[JsonIgnore]
		public DateTime DateUpdatedUTC { get; set; }

		//Date the deployment was built
		[JsonIgnore]
		public DateTime DateBuiltUTC { get; set; }

		//Date the deployment was deployed
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


		//Returns the UTC date for prepared completion in ticks
		[JsonProperty("datePreparedUTCTicksUnix")]
		public double DatePreparedUTCTicksUnix
		{
			get
			{
				return DatePreparedUTC.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; ;
			}
		}

		//Returns the UTC date for time of update in ticks
		[JsonProperty("dateUpdatedUTCTicksUnix")]
		public double DateUpdatedUTCTicksUnix
		{
			get
			{
				return DateUpdatedUTC.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; ;
			}
		}

		//Returns the UTC date for build date in ticks
		[JsonProperty("dateBuiltUTCTicksUnix")]
		public double DateBuiltUTCTicksUnix
		{
			get
			{
				return DateBuiltUTC.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; ;
			}
		}

		//Returns the UTC date of the deployment in ticks
		[JsonProperty("dateDeployedUTCTicksUnix")]
		public double DateDeployedUTCTicksUnix
		{
			get
			{
				return DateDeployedUTC.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; ;
			}
		}
	}


}