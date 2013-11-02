using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	//Enumerator representing deployment state
	public enum DeploymentStatus : byte
	{
		NoDeployments = 0,
		POSTReceived = 1,
		Deserializing = 2,
		DeserializingFailed = 3,
		ProcessingCommits = 4,
		ProcessingCommitsFailed = 5,
		OpeningRepo = 6,
		UpdatingRepo = 7,
		UpdatingRepoFailed = 8,
		Building = 9,
		BuildingFailed = 10,
		Starting = 11,
		Deployed = 12
	}
}