using ForeverDeploy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	public class FDContext : DbContext
	{
		public FDContext()
			: base("DefaultConnection")
		{
		}

		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<Commit> Commits { get; set; }
	}
}