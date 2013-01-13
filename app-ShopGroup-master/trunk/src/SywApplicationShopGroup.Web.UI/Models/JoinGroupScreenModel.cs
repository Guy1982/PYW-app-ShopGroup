using System.Collections.Generic;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Repositorys;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class JoinGroupScreenModel : AbstractHomeScreenModel
    {
        public long MemberId { get; set; }
        public long GroupId { get; set; }
        public string MemberName { get; set; }
        public string Token { get; set; }
        public string GroupName { get; set; }
        public JoinStatus Status { get; set; }
        public PageId PageId { get; set; }

        public JoinGroupScreenModel()
        {
            PageId = PageId.JoinAgroup;
        }
        //Used in the main page layout
        public IList<ShopGroup> ShopGroups { get; set; }

        public override IList<ShopGroup> GetGroups()
        {
            return ShopGroups;
        }
        public override long UserId()
        {
            return MemberId;
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