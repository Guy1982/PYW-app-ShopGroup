using System;
using SywApplicationShopGroup.Domain.Auth;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class LandingPageViewModel
    {
        public string AppLoginUrl { get; set; }
        public UserState UserState { get; set; }
        public Uri DashboardUrl { get; set; }
        public string UserName { get; set; }

        public bool DisplayProperConfiguraionMessage { get; set; }
        public long AppId { get; set; }
        public Uri ShopYourWayUrl { get; set; }

        public Uri LoginPageUrl { get { return new Uri(ShopYourWayUrl, "app/" + AppId + "/login"); } }
        public Uri LandingPageUrl { get { return new Uri(ShopYourWayUrl, "app/" + AppId + "/l"); } }
    }
}