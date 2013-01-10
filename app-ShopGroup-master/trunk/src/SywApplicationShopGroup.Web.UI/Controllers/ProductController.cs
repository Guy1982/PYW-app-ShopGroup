using System;
using System.Web.Mvc;
using Platform.Client;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Domain.WallPublish;
using SywApplicationShopGroup.Web.UI.Models;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAuthApi _authApi;
        private readonly IUsersApi _usersApi;
        private readonly IShopGroupRepository _shopGroupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IShopGroupFromInputValidator _shopGroupFromInputValidator;
        private readonly IShopGroupWallPublishApi _shopGroupWallPublishApi;

        
        
        public  ProductController(IShopGroupRepository shopGroupRepository, IPlatformTokenProvider platformTokenProvider,IUsersApi usersApi,
            IShopGroupWallPublishApi shopGroupWallPublishApi, IAuthApi authApi, IGroupMemberRepository groupMemberRepository, IShopGroupFromInputValidator shopGroupFromInputValidator)

        {
            _authApi = authApi;
            _usersApi = usersApi;
            _shopGroupRepository = shopGroupRepository;
            _platformTokenProvider = platformTokenProvider; 
            _shopGroupFromInputValidator = shopGroupFromInputValidator;
            _groupMemberRepository = groupMemberRepository;
            _shopGroupWallPublishApi = shopGroupWallPublishApi;
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

            if (!_shopGroupFromInputValidator.IsInputValid(formValue, new[]{InputFiled.Token, InputFiled.ProductId, InputFiled.GroupName})) return View("GroupCreateError");

            var tokenStr = _shopGroupFromInputValidator.GetStringKey(InputFiled.Token, formValue);
            _platformTokenProvider.Set(tokenStr);
            
            var user = _usersApi.Current();
            if (user == null) return View("GroupCreateError");
       
            try
            {
                var adminUser = _groupMemberRepository.GroupMember(user.Id);
                var productId = _shopGroupFromInputValidator.GetLongKey( InputFiled.ProductId, formValue);
                var shopGroupName = _shopGroupFromInputValidator.GetStringKey(InputFiled.GroupName, formValue);
                var newShopGroup = new ShopGroup
                                       {
                                           Name = shopGroupName,
                                           Admin = adminUser,
                                           ProductId = productId
                                       };
                _shopGroupRepository.AddOrSaveShopGroup(newShopGroup);
                _shopGroupWallPublishApi.PublishNewShopGroupStroy(newShopGroup);

                return View("PostGroupCreate", GenerateModelForProductId(productId));

            }
            catch
            {

                return View("GroupCreateError");
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
                    UserState =  _authApi.GetUserState(),
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
