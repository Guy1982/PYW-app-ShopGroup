using System;
using System.Collections.Generic;
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

        public ShopGroupWallPublishApi(IWallPublishApi wallPublishApi, IProductsApi productsApi)
        {
            _wallPublishApi = wallPublishApi;
            _productsApi = productsApi;
        }

        public void PublishNewShopGroupStroy(ShopGroup group)
        {
            if (group == null) return;

            var adminUser = group.Admin;
            var productList = _productsApi.Get(new List<long> { group.ProductId });
            _wallPublishApi.PublishStory("Shop Group", adminUser.Name + " had created new Shop Group: " + group.Name, productList[0].ImageUrl);
        }

        public void SendWallPsotToFriends(ShopGroup group, GroupMember member)
        {
            throw new NotImplementedException();
        }
    }
}
