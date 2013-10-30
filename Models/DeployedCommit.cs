using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForeverDeploy.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ForeverDeploy.Models
{
	[Table("DeployedCommits")]
	public class DeployedCommit
	{
		public DeployedCommit() { }

		public DeployedCommit(Commit commit)
		{
			Author = commit.author;
			Message = commit.message;
			RawNode = commit.raw_node;
			UtcTimeStamp = commit.utctimestamp;
			RawAuthor = commit.raw_author;
			Branch = commit.branch;
		}

		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		//Authors username
		[JsonProperty("author")]
		public string Author { get; set; }

		//Commit message
		[JsonProperty("message")]
		public string Message { get; set; }

		//Commit node
		[JsonIgnore]
		public string RawNode { get; set; }

		[JsonIgnore]
		public string UtcTimeStamp { get; set; }

		//Authors name and email
		[JsonProperty("rawAuthor")]
		public string RawAuthor { get; set; }

		[JsonProperty("branch")]
		public string Branch { get; set; }

		//Gets the authors name
		[JsonProperty("authorName")]
		public string AuthorName
		{
			get
			{
				if (!String.IsNullOrEmpty(RawAuthor))
				{
					var emailStart = RawAuthor.IndexOf('<');
					var emailEnd = RawAuthor.IndexOf('>');
					return RawAuthor.Substring((emailStart >= 0 ? emailStart + 1 : 0), (emailEnd >= 0 ? emailEnd - emailStart - 1 : 0));
				}
				return null;
			}
		}

		//Gets the authors email
		[JsonProperty("authorEmail")]
		public string AuthorEmail
		{
			get
			{
				if (!String.IsNullOrEmpty(RawAuthor))
				{
					var emailStart = RawAuthor.IndexOf('<');
					var emailEnd = RawAuthor.IndexOf('>');
					return RawAuthor.Substring(0, (emailStart >= 0 ? emailStart : 1) - 1);
				}
				return null;
			}
		}

		//Get formated age string
		[JsonIgnore]
		public string CommitAge
		{
			get
			{
				return DateCommited.TimeDiffAgeSingleUnit();
			}
		}

		//Get utc date commit was created in ticks
		[JsonIgnore]
		public DateTime DateCommited
		{
			get
			{
				return (Convert.ToDateTime(UtcTimeStamp)).AddHours(5);
			}
		}


	}
}