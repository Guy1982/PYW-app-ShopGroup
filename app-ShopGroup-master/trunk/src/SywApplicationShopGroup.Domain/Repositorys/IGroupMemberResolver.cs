using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Client;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Users;

namespace SywApplicationShopGroup.Domain.Repositorys
{
    public enum JoinStatus
    {
        NewAction,
        Success,
        FailGeneral,
        FailGroupNotFound,
        FailUserAlreadyAmember,
        FailUserAnonymous,
        FailUserNeedToInstallApp,
        FailUserIsntConnectedToGroup,
        FailPrivaceSettings

    }

    public interface IGroupMemberResolver
    {
        JoinStatus JoinUserToGroup(int groupId);
        JoinStatus JoinUserToGroup(long userId, int groupId);
        GroupMember GetGroupMember(long userId, bool createIfNeeded);
    }

    public class GroupMemberResolver : IGroupMemberResolver
    {
        private readonly IUsersApi _usersApi;
        private readonly IShopGroupRepository _shopGroupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IAuthApi _authApi;

        public GroupMemberResolver(IUsersApi usersApi, IShopGroupRepository shopGroupRepository, IGroupMemberRepository groupMemberRepository,
            IPlatformTokenProvider platformTokenProvider, IAuthApi authApi)
        {
            _usersApi = usersApi;
            _shopGroupRepository = shopGroupRepository;
            _groupMemberRepository = groupMemberRepository;
            _platformTokenProvider = platformTokenProvider;
            _authApi = authApi;

        }

        public GroupMember GetGroupMember(long userId, bool createIfNeeded)
        {
            var groupMember = _groupMemberRepository.GroupMember(userId);
            if (groupMember == null && createIfNeeded)
            {
                var users = _usersApi.Get(new List<long> { userId });
                foreach (var userDto in users)
                {
                    groupMember = new GroupMember
                                          {
                                              Name = userDto.Name,
                                              SywId = userDto.Id
                                          };
                    _groupMemberRepository.AddOrSaveNewGroupMember(groupMember);
                }
            }
            return groupMember;
        }

        public JoinStatus JoinUserToGroup(int groupId)
        {
            try
            {
                if(_authApi.GetUserState() != UserState.Authorized) return JoinStatus.FailUserNeedToInstallApp;
                
                var currentUser = _usersApi.Current();
                if(currentUser == null) return JoinStatus.FailGeneral;
                return JoinUserToGroup(currentUser.Id, groupId);
            }
            catch
            {
                return JoinStatus.FailGeneral;
            }
        }

        public JoinStatus JoinUserToGroup(long userId, int groupId)
        {
            try
            {
                var currentUser = _usersApi.Current();
                var testResult = CanUserBeAddedToGroup(groupId, currentUser.Id);
                if (testResult != JoinStatus.Success) return testResult;

                var groupMemebr = _groupMemberRepository.GroupMember(currentUser.Id) ??
                                  new GroupMember
                                  {
                                      SywId = currentUser.Id,
                                      Name = currentUser.Name,
                                      Token = _platformTokenProvider.Get()
                                  };
                var group = _shopGroupRepository.GetShoupGroup(groupId);
                group.AddMember(groupMemebr);
                _groupMemberRepository.AddOrSaveNewGroupMember(groupMemebr);

                return JoinStatus.Success;
            }
            catch(Exception e)
            {

                return JoinStatus.FailGeneral;
            }
        }


        private JoinStatus CanUserBeAddedToGroup(int groupId, long userId)
        {
            var searchResult = JoinStatus.FailUserIsntConnectedToGroup;
            try
            {
                var group = _shopGroupRepository.GetShoupGroup(groupId);
                if (group == null) return JoinStatus.FailGroupNotFound;
                if (@group.Members.Any(q => q.SywId == userId)) return JoinStatus.FailUserAlreadyAmember;


                foreach (var member in group.Members)
                {
                    try
                    {


                        var followers = _usersApi.GetFollowers(member.SywId);
                        if (followers.Any(follower => follower.Id == userId))
                        {
                            return JoinStatus.Success;
                        }


                        var followings = _usersApi.GetFollowing(member.SywId);
                        if (followings.Any(following => following.Id == userId))
                        {
                            return JoinStatus.Success;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception.InnerException.GetType() == typeof(System.Net.WebException))
                        {
                            searchResult = JoinStatus.FailPrivaceSettings;   
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

            }
            catch (Exception )
            {
                return JoinStatus.FailGeneral;
            }


            return searchResult;
        }


    }
}
