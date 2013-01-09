using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly IApplicationSettings _applicationSettings;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IUsersApi _usersApi;
        private readonly IPlatformSettings _platformSettings;
        private readonly IShopGroupRepository _shopGroupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IProductsApi _productsApi;
        private readonly IAuthApi _authApi;



        public HomeController(IShopGroupRepository shopGroupRepository, IPlatformTokenProvider platformTokenProvider,IUsersApi usersApi,
            IApplicationSettings applicationSettings, IProductsApi productsApi, IAuthApi authApi, IPlatformSettings platformSettings, IGroupMemberRepository groupMemberRepository)
        {
    
            _applicationSettings = applicationSettings;
            _platformTokenProvider = platformTokenProvider;
            _platformSettings = platformSettings;
            _usersApi = usersApi;
            _shopGroupRepository = shopGroupRepository;
            _groupMemberRepository = groupMemberRepository;
            _productsApi = productsApi;
            _authApi = authApi;

        }

        public ActionResult Index()
        {
            
            if (_platformTokenProvider.Get() == null || _authApi.GetUserState() != UserState.Authorized)
                return Redirect("/landing");

            var currentUser = _usersApi.Current();
            var userToken = _platformTokenProvider.Get();
            var groupMember = _groupMemberRepository.GroupMember((int)currentUser.Id);
           
            if (groupMember == null) 
                ValidateGroupMember(out groupMember);
            
            groupMember.Token = userToken;
            _groupMemberRepository.AddOrSaveNewGroupMember(groupMember);

            var currentUserFollowing = _usersApi.GetFollowing(currentUser.Id);
            var currentUserFollowers = _usersApi.GetFollowers(currentUser.Id);

            return View(ToModel(currentUser, null, currentUserFollowing, currentUserFollowers));
        }

        private void ValidateGroupMember(out GroupMember groupMember)
        {
            var currentUser = _usersApi.Current();
            groupMember = new GroupMember
                              {
                                  Name = currentUser.Name,
                                  SywId = currentUser.Id
                              };
        }

        public ActionResult ShowShopGroup(int shopGroupId, int userId)
        {
            var groupMember = _groupMemberRepository.GroupMember(userId);
            _platformTokenProvider.Set(groupMember.Token);

            var currentUser = _usersApi.Current();
            var currentUserFollowing = _usersApi.GetFollowing(currentUser.Id);
            var currentUserFollowers = _usersApi.GetFollowers(currentUser.Id);
            var group = _shopGroupRepository.GetShoupGroup(shopGroupId);
            var currentProduct = _productsApi.Get(new[] { group.ProductId }).FirstOrDefault();

            return View(ToModel(currentUser, currentProduct, currentUserFollowing, currentUserFollowers));
        }

        private ShopGroupHomeScreenModel ToModel(UserDto userDto, ProductDto product, IEnumerable<UserDto> following = null, IEnumerable<UserDto> followers = null)
        {
            var groupMember = _groupMemberRepository.GroupMember((int)userDto.Id);

            return new ShopGroupHomeScreenModel
                {
                    Id = userDto.Id,
                    MemberName = userDto.Name,
                    ImageUrl = userDto.ImageUrl,
                    Product = product,
                    ProfileUrl = new Uri(_platformSettings.SywWebSiteUrl, userDto.ProfileUrl),
                    Following = following == null ?
                        new ShopGroupHomeScreenModel[0] :
                        following.Select(x => ToModel(x, product)).ToArray(),

                    Followers = followers == null ?
                           new ShopGroupHomeScreenModel[0] :
                           followers.Select(x => ToModel(x, product)).ToArray(),

                    ShopGroups = groupMember != null ?
                            groupMember.Groups :
                            null
                };
        }
    }
}
