using System;
using System.Linq;
using System.Web.Mvc;
using Platform.Client;
using Platform.Client.Configuration;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Products;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Web.UI.Models;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IUsersApi _usersApi;
        private readonly IPlatformSettings _platformSettings;
        private readonly IShopGroupRepository _shopGroupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IProductsApi _productsApi;
        private readonly IAuthApi _authApi;
        private readonly ITokenResolver _tokenResolver;
        private readonly IGroupMemberResolver _groupMemberResolver;


        public HomeController(IShopGroupRepository shopGroupRepository, IPlatformTokenProvider platformTokenProvider,IUsersApi usersApi, ITokenResolver tokenResolver,
             IProductsApi productsApi, IAuthApi authApi, IPlatformSettings platformSettings, IGroupMemberRepository groupMemberRepository, IGroupMemberResolver groupMemberResolver)
        {

            _platformTokenProvider = platformTokenProvider;
            _platformSettings = platformSettings;
            _usersApi = usersApi;
            _shopGroupRepository = shopGroupRepository;
            _groupMemberRepository = groupMemberRepository;
            _productsApi = productsApi;
            _authApi = authApi;
            _tokenResolver = tokenResolver;
            _groupMemberResolver = groupMemberResolver;
        }

        public ActionResult Index()
        {
            
            if (_platformTokenProvider.Get() == null || _authApi.GetUserState() != UserState.Authorized)
                return Redirect("/landing");

            var currentUser = _usersApi.Current();
             var groupMember = _groupMemberResolver.GetGroupMember(currentUser.Id, true);

            _tokenResolver.IsTokenResolvedForUser(currentUser.Id);
        
            return View(ToModel(groupMember, currentUser, null));
        }

       

        public ActionResult ShowShopGroup(int shopGroupId, int userId)
        {
            var groupMember = _groupMemberRepository.GroupMember(userId);
            _platformTokenProvider.Set(groupMember.Token);

            var currentUser = _usersApi.Current();
           var group = _shopGroupRepository.GetShoupGroup(shopGroupId);
            var currentProduct = _productsApi.Get(new[] { group.ProductId }).FirstOrDefault();

            return View(ToModel(groupMember,currentUser, currentProduct));
        }

        private ShopGroupHomeScreenModel ToModel(GroupMember groupMember, UserDto userDto, ProductDto product )
        {
            return new ShopGroupHomeScreenModel
                {
                    Id = userDto.Id,
                    MemberName = userDto.Name,
                    ImageUrl = userDto.ImageUrl,
                    Product = product,
                    Token = _platformTokenProvider.Get(),
                    ProfileUrl = new Uri(_platformSettings.SywWebSiteUrl, userDto.ProfileUrl),
                    
                    ShopGroups = groupMember != null ?
                            groupMember.Groups :
                            null
                };
        }
    }
}
