using System;
using System.Collections.Generic;
using System.Text;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Products;


namespace SywApplicationShopGroup.Domain.WallPublish
{
    public interface IShopGroupWallPublishApi
    {
        void PublishNewShopGroupStroy(ShopGroup group);
        void SendWallPsotToFriends(ShopGroup group, GroupMember member);
    }

    public class ShopGroupWallPublishApi :IShopGroupWallPublishApi
    {
        private readonly IWallPublishApi _wallPublishApi;
        private readonly IProductsApi _productsApi;
        private readonly Routes _routes;

        public ShopGroupWallPublishApi(Routes routes, IWallPublishApi wallPublishApi, IProductsApi productsApi)
        {
            _wallPublishApi = wallPublishApi;
            _productsApi = productsApi;
            _routes = routes;
        }

        public void PublishNewShopGroupStroy(ShopGroup group)
        {
            if (group == null) return;

            var adminUser = group.Admin;
            var productList = _productsApi.Get(new List<long> { group.ProductId });
            
            var html = new StringBuilder();
            var link = "[Join Group Now](" + _routes.JoinGroup(group.Id)+ ")";
            var result = _wallPublishApi.PublishStory("Shop Group", adminUser.Name + " had created new Shop Group: " + group.Name +". "+ link , productList[0].ImageUrl);
        }

        public void SendWallPsotToFriends(ShopGroup group, GroupMember member)
        {
            throw new NotImplementedException();
        }
    }
}
