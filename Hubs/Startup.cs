using Microsoft.Owin;
using Owin;

namespace ForeverDeploy
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}