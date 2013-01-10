using System.Collections.Generic;
using System.Linq;
using SywApplicationShopGroup.Domain.Entities;

namespace SywApplicationShopGroup.Domain.Repositorys
{
    public interface IShopGroupRepository
    {
        void AddOrSaveShopGroup(ShopGroup group);
        ShopGroup GetShoupGroup(int groupId);
        IList<ShopGroup> GetAllShopGroups();

    }

    public class ShopGroupRepository : IShopGroupRepository
    {
        private readonly ISessionProvider _sessionProvider;

        public ShopGroupRepository(ISessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }
       
        public void AddOrSaveShopGroup(ShopGroup group)
        {
            _sessionProvider.WithSession(session => session.SaveOrUpdate(group));
        }

        public ShopGroup GetShoupGroup(int groupId)
        {
            var shopGroup = _sessionProvider.Query<ShopGroup>(q => q.Where(x => x.Id == groupId)).FirstOrDefault();
            return shopGroup;
        }

        public IList<ShopGroup> GetAllShopGroups()
        {
            return _sessionProvider.Query<ShopGroup>(q => q);
        }

    }
  
}
