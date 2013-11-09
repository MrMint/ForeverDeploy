using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ForeverDeploy.Extensions;

namespace ForeverDeploy.Models
{
	//These classes represent the json payload delivered via bitbucket
	public class Commit
	{
		public string node { get; set; }
		public List<File> files { get; set; }
		public string branch { get; set; }
		public string utctimestamp { get; set; }
		public string timestamp { get; set; }
		public string raw_node { get; set; }
		public string message { get; set; }
		public int size { get; set; }
		public string author { get; set; }
		public List<string> parents { get; set; }
		public string raw_author { get; set; }
		public object revision { get; set; }
	}

	public class File
	{
		public string type { get; set; }
		public string file { get; set; }
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