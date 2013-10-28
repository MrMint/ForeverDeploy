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
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int DateDeployed { get; set; }

		public DeploymentStatus DeploymentStatus { get; set; }

		//Commit foreign key
		public int CommitId { get; set; }
		
		//Lazy loaded commit object
		public virtual DeployedCommit Commit { get; set; }
	}

	//Enumerator representing deployment state
	public enum DeploymentStatus : byte
	{
		Pulling,
		Compiling,
		Starting,
		Deployed
	}
}