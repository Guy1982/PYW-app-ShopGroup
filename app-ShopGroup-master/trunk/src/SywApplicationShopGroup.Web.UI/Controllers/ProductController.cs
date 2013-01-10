using System;
using System.Web.Mvc;
using Platform.Client;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Domain.WallPublish;
using SywApplicationShopGroup.Web.UI.Models;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUsersApi _usersApi;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IShopGroupFromInputValidator _shopGroupFromInputValidator;
        private readonly IShopGroupBuilder _shopGroupBuilder;

        public ProductController(IPlatformTokenProvider platformTokenProvider, IUsersApi usersApi, IShopGroupBuilder shopGroupBuilder,
              IShopGroupFromInputValidator shopGroupFromInputValidator)
        {
            _usersApi = usersApi;
            _platformTokenProvider = platformTokenProvider;
            _shopGroupFromInputValidator = shopGroupFromInputValidator;
            _shopGroupBuilder = shopGroupBuilder;
        }


        public ActionResult Index(long productId)
        {
            return View(GenerateModelForProductId(productId));
        }

        public ActionResult CreateShopGroup(string token, long productId)
        {
            _platformTokenProvider.Set(token);
            return View(GenerateModelForProductId(productId));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateShopGroupAction(FormCollection formValue)
        {
           try
            {
                var result = _shopGroupBuilder.CreateNewShopGroup(formValue);

                if (result != ShopGroupBuilder.BuildShopGroupResult.Success)
                    return View("GroupCreateError", new BuildNewGroupErrorViewModel { ErrorCode = result }); 
                
               var productId = _shopGroupFromInputValidator.GetLongKey(InputFiled.ProductId, formValue);
               return View("PostGroupCreate", GenerateModelForProductId(productId));

            }
            catch
            {

                return View("GroupCreateError", new BuildNewGroupErrorViewModel{ErrorCode = ShopGroupBuilder.BuildShopGroupResult.FailGeneral});
            }

        }

        private ProductViewModel GenerateModelForProductId(long productId)
        {
            try
            {

                var user = _usersApi.Current();
                return new ProductViewModel
                {
                    UserName = user.Name,
                    UserId = user.Id,
                    ProductId = productId,
                    SessionToken = _platformTokenProvider.Get()
                };
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
