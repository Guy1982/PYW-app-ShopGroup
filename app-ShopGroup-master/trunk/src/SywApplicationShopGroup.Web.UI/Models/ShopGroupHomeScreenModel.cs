﻿using System;
using System.Collections.Generic;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Products;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class ShopGroupHomeScreenModel : AbstractHomeScreenModel
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string Token { get; set; }
        public string ImageUrl { get; set; }
        public Uri ProfileUrl { get; set; }
        public IList<ShopGroupHomeScreenModel> Followers { get; set; }
        public IList<ShopGroupHomeScreenModel> Following { get; set; }
        public IList<ShopGroup> ShopGroups { get; set; }
        public ProductDto Product { get; set; }

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
    }
}