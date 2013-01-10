using System;
using System.Configuration;
using Platform.Client.Configuration;

namespace SywApplicationShopGroup.Domain.Configuration
{
	public class PlatformSettings : IPlatformSettings
	{
		public Uri SywWebSiteUrl { get { return Config.GetUri("platform:syw-site-url"); } }
		public Uri ApiUrl { get { return Config.GetUri("platform:api-url"); } }
		public Uri SecureApiUrl { get { return Config.GetUri("platform:secured-api-url"); } }
        public string SywAppLoginUrl { get { return Config.GetString("platform:syw-app-login-url"); } }
        public string SywAppJoinGroupUrl { get { return Config.GetString("platform:syw-app-joinGroup-url"); } }

        

	}
}
