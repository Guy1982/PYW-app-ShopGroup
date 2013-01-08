﻿using System.Web.Mvc;
using Platform.Client.Common.Context;
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


	    public LandingController()
	    {
            _routes = new Routes();
            var contextProvider = new HttpContextProvider();
            _authApi = new AuthApi(contextProvider);
            _usersApi = new UsersApi(contextProvider);
            _appActionsApi = new AppActionsApi(contextProvider);
	    }

	    public ActionResult Index()
	    {

            var userState = _authApi.GetUserState();

            var model = new LandingPageViewModel
            {
                AppLoginUrl = _routes.Login(),
                UserState = userState 
            };

            if (model.UserState != UserState.Anonymous)
                model.UserName = _usersApi.Current().Name;


            // This should be done only once - by a utility or service (using offline token of course). No need to do it for every user.
            // For the purpose of the demonstration, this will be fine here (remember - big NO NO in production apps)
            _appActionsApi.Register("Create Shop Group", 300, 300);

            return View(model);
        }

    }

}