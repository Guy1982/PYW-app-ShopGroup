using System;
using System.Web.Mvc;
using SywApplicationShopGroup.Domain;
using SywApplicationShopGroup.Domain.AppActions;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Web.UI.Models;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class LandingController : Controller
    {
        private readonly Routes _routes;
        private readonly IAuthApi _authApi;
        private readonly IUsersApi _usersApi;
        private readonly IAppActionsApi _appActionsApi;


        public LandingController(Routes routes, IAuthApi authApi, IUsersApi usersApi, IAppActionsApi appActionsApi)
        {
            _routes = routes;
            _authApi = authApi;
            _usersApi = usersApi;
            _appActionsApi = appActionsApi;
	    }

	    public ActionResult Index()
	    {
	        LandingPageViewModel model;
	        try
	        {
                var userState = _authApi.GetUserState();

                model = new LandingPageViewModel
                {
                    AppLoginUrl = _routes.Login(),
                    UserState = userState
                };

                if (model.UserState != UserState.Anonymous)
                    model.UserName = _usersApi.Current().Name;

            }
	        catch (Exception)
	        {
                model = null;
	        }
                   

            return View(model);
        }

    }

}
