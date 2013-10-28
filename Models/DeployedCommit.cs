using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForeverDeploy.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForeverDeploy.Models
{
	[Table("DeployedCommits")]
	public class DeployedCommit
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		
		//Authors username
		public string author { get; set; }
		
		//Commit message
		public string message { get; set; }
		
		//Commit node
		public string raw_node { get; set; }

		public string utctimestamp { get; set; }
		//Authors name and email
		public string raw_author { get; set; }

		//Get formated age string
		public string CommitAge
		{
			get
			{
				return DateCommited.TimeDiffAgeSingleUnit();
			}
		}

		//Get utc date commit was created
		public DateTime DateCommited
		{
			get
			{
				return (Convert.ToDateTime(utctimestamp)).AddHours(5);
			}
		}
	}
}