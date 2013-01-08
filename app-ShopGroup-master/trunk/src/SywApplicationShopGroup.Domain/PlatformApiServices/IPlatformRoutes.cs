using System;
using Platform.Client.Configuration;

namespace SywApplicationShopGroup.Domain.PlatformApiServices
{
	public interface IPlatformRoutes
	{
		string RewardsLogin { get; set; }
		string Login { get; set; }
		string RegularCanvas { get; set; }
		string LandingPage { get; set; }
	}

	public class PlatformRoutes : IPlatformRoutes
	{
		public string RewardsLogin { get; set; }
		public string Login { get; set; }
		public string RegularCanvas { get; set; }
		public string LandingPage { get; set; }

		public PlatformRoutes(IApplicationSettings applicationSettings,IPlatformSettings platformSettings)
		{
			//RewardsLogin = platformSettings.PlatformHomePage + String.Format("/secured/app/{0}/rewards-sign-up", applicationSettings.AppId);
			Login = platformSettings.SywWebSiteUrl + String.Format("/secured/app/{0}/login", applicationSettings.AppId);
			RegularCanvas = String.Format("/app/{0}/r/", applicationSettings.AppId);
			LandingPage = String.Format("/app/{0}/l/", applicationSettings.AppId);
		}
	}
}
