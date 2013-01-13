using System;
using System.Collections.Generic;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Products;
using SywApplicationShopGroup.Domain.Users;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class ShopGroupHomeScreenModel : AbstractHomeScreenModel
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string Token { get; set; }
        public string ImageUrl { get; set; }
        public Uri ProfileUrl { get; set; }
        public IList<UserDto> Followers { get; set; }
        public IList<UserDto> Following { get; set; }
        public IList<ShopGroup> ShopGroups { get; set; }
        public ProductDto Product { get; set; }
        public ShopGroup Group { set; get; }
        public PageId PageId { set; get; }
        public IDictionary<string, string> DiscountTable { get; private set; } 

        public ShopGroupHomeScreenModel()
        {
            PageId = PageId.MyGroups;
            DiscountTable = new Dictionary<string, string>()
                                {
                                    {"5-10", "300 Shop Your Way Points"},
                                    {"11-15", "500 Shop Your Way Points"},
                                    {"16-25", "5% extra discount"},
                                    {"25 or more", "10% discount"}
                                };
        }
        public override IList<ShopGroup> GetGroups()
        {
            return ShopGroups;
        }

        public override long UserId()
        {
            return Id;
        }

        public override string SessionToken()
        {
            return Token;
        }

        public override PageId GetPageId()
        {
            return PageId;
        }
    }
}