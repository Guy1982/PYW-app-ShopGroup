using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Platform.Client;
using Platform.Client.Common.Context;
using Platform.Client.Configuration;
using SywApplicationShopGroup.Domain.Configuration;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.PlatformApiServices;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class PostLoginController : Controller
    {
        private readonly IPlatformProxy _platformProxy;
		private readonly IApplicationSettings _applicationSettings;
		private readonly PlatformTokenProvider _platformTokenProvider;
	    private readonly IPlatformSettings _platformSettings;
        private readonly IPlatformRoutes _platformRoutes;
        private readonly IUsersApi _usersApi;
        private readonly IGroupMemberRepository _groupMemberRepository;

	    public PostLoginController()
	    {
            _platformProxy = new PlatformProxy(new PlatformSettings(),
                                      new ApplicationSettings(),
                                      new PlatformTokenProvider(new HttpContextProvider()));
		    _applicationSettings = new ApplicationSettings();
		    _platformSettings = new PlatformSettings();
		    _platformTokenProvider = new PlatformTokenProvider(new HttpContextProvider());
            _platformRoutes = new PlatformRoutes(new ApplicationSettings(), new PlatformSettings());
            _usersApi = new UsersApi(new HttpContextProvider());
            _groupMemberRepository = new GroupMemberRepository();
	    }

	    public ActionResult Index()
	    {
		    long appId;

			// Making sure the application is configured correctly and the application is called from a canvas
			// Delete this code once this is done
		    try
		    {
			    appId = _applicationSettings.AppId;
		    }
		    catch (ConfigurationErrorsException)
		    {
			    return Redirect("/landing");
		    }

			if (_platformTokenProvider.Get() == null)
				return Redirect("/landing");

	        AddUserToDataBase();
	        RegisterAppLink();
	        AddProductButton();

            return View(new PostLoginModel(appId, _platformSettings.SywWebSiteUrl));
        }

        private void AddUserToDataBase()
        {
            var user = _usersApi.Current();
            var token = _platformTokenProvider.Get();
            if(_groupMemberRepository.GroupMember((int)user.Id) != null) return;         
          
            var groupMember = new GroupMember { Name = user.Name, SywId = (int)user.Id, Token = token};
            _groupMemberRepository.AddOrSaveNewGroupMember(groupMember);

        }

        private void RegisterAppLink()
        {
            var appLinkParams = new[]
			                    	{
			                    		new KeyValuePair<string, object>("title", "Shop Group"),
			                    		new KeyValuePair<string, object>("url", _platformRoutes.RegularCanvas)
			                    	};


            _platformProxy.Get<string>("/applinks/register", appLinkParams);
        }

        private void AddProductButton()
        {
            var appLinkParams = new[]
			                    	{
			                    		new KeyValuePair<string, object>("text", "Shop Group"),
			                    		new KeyValuePair<string, object>("canvasHeight", "300"),
                                        new KeyValuePair<string, object>("canvasWidth", "300"),
			                    		new KeyValuePair<string, object>("level", "Product")
			                    	};


            _platformProxy.Get<string>("/app-actions/product/register", appLinkParams);
        }
    }

	public class PostLoginModel
	{
		public PostLoginModel(long appId, Uri shopYourWayUrl)
		{
			CanvasPageUrl = new Uri(shopYourWayUrl, "app/" + appId + "/r");
		}

		public Uri CanvasPageUrl { get; private set; }
	}
}
