using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Extensions
{
	public static class Extensions
	{
		//Returns a string x y ago from a datetime
		public static string TimeDiffAgeSingleUnit(this DateTime startTime)
		{
			var age = (DateTime.UtcNow - startTime);
			string ageString = "";
			if (age.Days >= 1)
			{
				ageString = ((int)age.Days).ToString() + " days ago";
			}
			else
				if (age.Hours >= 1)
				{
					ageString = ((int)age.Hours).ToString() + " hours ago";
				}
				else
					if (age.Minutes >= 1)
					{
						ageString = ((int)age.Minutes).ToString() + " minutes ago";
					}
					else
						if (age.Seconds >= 1)
						{
							ageString = ((int)age.Seconds).ToString() + " seconds ago";
						}
			return ageString;
		}


		public static string TimeDiffRemainSingleUnit(this TimeSpan age)
		{
			string ageString = "";
			if (age.Days >= 1)
			{
				ageString = ((int)age.Days).ToString() + " days remaining";
			}
			else
				if (age.Hours >= 1)
				{
					ageString = ((int)age.Hours).ToString() + " hours remaining";
				}
				else
					if (age.Minutes >= 1)
					{
						ageString = ((int)age.Minutes).ToString() + " minutes remaining";
					}
					else
						if (age.Seconds >= 1)
						{
							ageString = ((int)age.Seconds).ToString() + " seconds remaining";
						}
			return ageString;
		}
	}

}