using System;
using System.Linq;
using Platform.Client;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;

namespace SywApplicationShopGroup.Domain.Auth
{
    public interface ITokenResolver
    {
        bool IsTokenResolvedForUser();
        bool IsTokenResolvedForUser(long userId);
    }

    public class TokenResolver : ITokenResolver
    {
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IGroupMemberResolver _groupMemberResolver;
        private readonly IUsersApi _usersApi;

        public TokenResolver(IPlatformTokenProvider platformTokenProvider, IGroupMemberRepository groupMemberRepository,
            IGroupMemberResolver groupMemberResolver, IUsersApi usersApi)
        {
            _usersApi = usersApi;
            _platformTokenProvider = platformTokenProvider;
            _groupMemberRepository = groupMemberRepository;
            _groupMemberResolver = groupMemberResolver;
        }
        public bool IsTokenResolvedForUser()
        {
            try
            {
                var token = _platformTokenProvider.Get();
                if (token == null || !token.Any()) return false;

                var currentUser = _usersApi.Current();
                AddTokenToUserIfNeeded(currentUser.Id, token);
                return true;
            }
            catch 
            {            
                return false;
            }
         
        }

        public bool IsTokenResolvedForUser(long userId)
        {
            var token = _platformTokenProvider.Get();
            if (token != null && token.Any())
            {
                AddTokenToUserIfNeeded(userId, token);
                return true;
            }
            var groupMember = _groupMemberResolver.GetGroupMember(userId, true);
            //try to get Token from user DB
            if (groupMember.Token == null) return false;

            _platformTokenProvider.Set(groupMember.Token);
            return true;
        }

        private void AddTokenToUserIfNeeded(long userId, string token)
        {
            var groupMember = _groupMemberResolver.GetGroupMember(userId, false);
            if (groupMember == null || groupMember.Token == token) return;
            groupMember.Token = token;
            _groupMemberRepository.AddOrSaveNewGroupMember(groupMember);
        }
    }
}
