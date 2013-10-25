using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ForeverDeploy.Extensions;

namespace ArrowManager.Models
{
	[Table("Commits")]
	public class Commit
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public string author { get; set; }
		public string message { get; set; }
		public string raw_node { get; set; }
		public string raw_author { get; set; }
		public List<File> files { get; set; }
		public string branch { get; set; }
		public string utctimestamp { get; set; }

		public string CommitAge
		{
			get
			{
				return DateCommited.TimeDiffAgeSingleUnit();
			}
		}

		public DateTime DateCommited
		{
			get
			{
				return (Convert.ToDateTime(utctimestamp)).AddHours(5);
			}
		}

		[NotMapped]
		public string timestamp { get; set; }
		[NotMapped]
		public int size { get; set; }
		[NotMapped]
		public string node
		{
			get
			{
				return raw_node.Substring(0, 7);
			}
		}
		[NotMapped]
		public List<string> parents { get; set; }
		[NotMapped]
		public object revision { get; set; }
	}
	[Table("Files")]
	public class File
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public int? CommitID { get; set; }
		public virtual Commit Commit { get; set; }
		public string type { get; set; }
		public string file { get; set; }
		public string fileName
		{
			get
			{
				return file.Substring(file.LastIndexOf('/') + 1, file.Length - file.LastIndexOf('/') - 1);
			}
		}
	}

	public class Repository
	{
		public string website { get; set; }
		public bool fork { get; set; }
		public string name { get; set; }
		public string scm { get; set; }
		public string owner { get; set; }
		public string absolute_url { get; set; }
		public string slug { get; set; }
		public bool is_private { get; set; }
	}

	public class RootObject
	{
		public Repository repository { get; set; }
		public bool truncated { get; set; }
		public List<Commit> commits { get; set; }
		public string canon_url { get; set; }
		public string user { get; set; }
	}
}