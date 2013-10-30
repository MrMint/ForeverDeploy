using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverDeploy.Models
{
	//Enumerator representing deployment state
	public enum DeploymentStatus : byte
	{
		NoDeployments,
		POSTReceived,
		Deserializing,
		DeserializingFailed,
		ProcessingCommits,
		ProcessingCommitsFailed,
		OpeningRepo,
		UpdatingRepo,
		UpdatingRepoFailed,
		Building,
		BuildingFailed,
		Starting,
		Deployed
	}
}