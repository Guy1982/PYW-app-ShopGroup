﻿using System;
using SywApplicationShopGroup.Domain.Configuration;

namespace SywApplicationShopGroup.Domain
{
	public class Routes
	{
		private readonly PlatformSettings _platformSettings;
		private readonly ApplicationSettings _applicationSettings;

		public Routes()
		{
			_platformSettings = new PlatformSettings();
			_applicationSettings = new ApplicationSettings();
		}

		public string Login()
		{
			return _platformSettings.SywWebSiteUrl + String.Format(_platformSettings.SywAppLoginUrl, _applicationSettings.AppId);
		}

        public string JoinGroup(int groupId)
        {
            return _platformSettings.SywWebSiteUrl + String.Format(_platformSettings.SywAppJoinGroupUrl, _applicationSettings.AppId, groupId);
        }

		public Uri Dashboard()
		{
			return new Uri(_platformSettings.SywWebSiteUrl,"/app/" + _applicationSettings.AppId + "/r");
		}
	}
}
