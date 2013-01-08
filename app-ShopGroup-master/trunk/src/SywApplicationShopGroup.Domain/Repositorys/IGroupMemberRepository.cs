
using System.Collections.Generic;
using System.Linq;
using SywApplicationShopGroup.Domain.Entities;


namespace SywApplicationShopGroup.Domain.Repositorys
{
    public interface IGroupMemberRepository
    {
        void AddOrSaveNewGroupMember(GroupMember member);
        IList<GroupMember> GetShoupGroupMembers(int groupId);
        GroupMember GetShoupGroupAdminMember(int groupId);
        IList<GroupMember> GetAllGroupsMembers();
        IList<GroupMember> GetAllGroupsAdminMembers();
        GroupMember GroupMember(long memberId);

    }

    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly SessionProvider _sessionProvider;

        public GroupMemberRepository()
        {
            _sessionProvider = new SessionProvider();
        }
       
        public void AddOrSaveNewGroupMember(GroupMember member)
        {
            _sessionProvider.WithSession(session => session.SaveOrUpdate(member));
        }

        public IList<GroupMember> GetShoupGroupMembers(int groupId)
        {
            var group = _sessionProvider.Query<ShopGroup>(q => q.Where(x => x.Id == groupId)).FirstOrDefault();
            return group == null ? null : group.Members;
        }

        public GroupMember GetShoupGroupAdminMember(int groupId)
        {
            var group = _sessionProvider.Query<ShopGroup>(q => q.Where(x => x.Id == groupId)).FirstOrDefault();
            return group == null ? null : group.Admin;
        }

        public IList<GroupMember> GetAllGroupsMembers()
        {
            return _sessionProvider.Query<GroupMember>(q => q);
        }

        public IList<GroupMember> GetAllGroupsAdminMembers()
        {
            return _sessionProvider.QueryOver<ShopGroup, GroupMember>(q => q.Select(x => x.Admin));
        }

        public GroupMember GroupMember(long memberId)
        {
            return _sessionProvider.Query<GroupMember>(q => q.Where(x => x.SywId == memberId)).FirstOrDefault();
        } 
      
    }
  
}
