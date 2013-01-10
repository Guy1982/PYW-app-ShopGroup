using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Platform.Client;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Domain.WallPublish;

namespace SywApplicationShopGroup.Domain.Repositorys
{
    public interface IShopGroupBuilder
    {

        ShopGroupBuilder.BuildShopGroupResult CreateNewShopGroup(FormCollection formValues);
    }

    public class ShopGroupBuilder : IShopGroupBuilder
    {
        public enum BuildShopGroupResult
        {
            Success,
            FailUserNotKnown,
            FailGeneral,
            FailInputError
        }

        private readonly IUsersApi _usersApi;
        private readonly IShopGroupRepository _shopGroupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IShopGroupFromInputValidator _shopGroupFromInputValidator;
        private readonly IShopGroupWallPublishApi _shopGroupWallPublishApi;



        public ShopGroupBuilder(IShopGroupRepository shopGroupRepository, IPlatformTokenProvider platformTokenProvider, IUsersApi usersApi,
            IShopGroupWallPublishApi shopGroupWallPublishApi, IGroupMemberRepository groupMemberRepository, IShopGroupFromInputValidator shopGroupFromInputValidator)

        {
            _usersApi = usersApi;
            _shopGroupRepository = shopGroupRepository;
            _platformTokenProvider = platformTokenProvider; 
            _shopGroupFromInputValidator = shopGroupFromInputValidator;
            _groupMemberRepository = groupMemberRepository;
            _shopGroupWallPublishApi = shopGroupWallPublishApi;
        }

        public BuildShopGroupResult CreateNewShopGroup(FormCollection formValues)
        {
            if (!_shopGroupFromInputValidator.IsInputValid(formValues, new[] { InputFiled.Token, InputFiled.ProductId, InputFiled.GroupName }))
                return BuildShopGroupResult.FailInputError;

            var tokenStr = _shopGroupFromInputValidator.GetStringKey(InputFiled.Token, formValues);
            _platformTokenProvider.Set(tokenStr);

            var user = _usersApi.Current();
            if (user == null) return BuildShopGroupResult.FailUserNotKnown;

            try
            {
                var adminUser = _groupMemberRepository.GroupMember(user.Id);
                var productId = _shopGroupFromInputValidator.GetLongKey(InputFiled.ProductId, formValues);
                var shopGroupName = _shopGroupFromInputValidator.GetStringKey(InputFiled.GroupName, formValues);
                var newShopGroup = new ShopGroup
                {
                    Name = shopGroupName,
                    Admin = adminUser,
                    ProductId = productId
                };
                _shopGroupRepository.AddOrSaveShopGroup(newShopGroup);
                _shopGroupWallPublishApi.PublishNewShopGroupStroy(newShopGroup);

                return BuildShopGroupResult.Success;

            }
            catch
            {

                return BuildShopGroupResult.FailGeneral;
            }
        }
    }
}