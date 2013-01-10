using System.Web.Mvc;
using Platform.Client;
using SywApplicationShopGroup.Domain.Auth;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Web.UI.Models;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public class JoinSgController : Controller
    {

        private readonly IUsersApi _usersApi;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly ITokenResolver _tokenResolver;
        private readonly IGroupIdProvider _groupIdProvider;
        private readonly IGroupMemberResolver _groupMemberResolver;
        private readonly IShopGroupFromInputValidator _shopGroupFromInputValidator;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IShopGroupRepository _shopGroupRepository;

        public JoinSgController(IUsersApi usersApi, IGroupMemberRepository groupMemberRepository,IPlatformTokenProvider platformTokenProvider,
            ITokenResolver tokenResolver, IGroupMemberResolver groupMemberResolver, IShopGroupFromInputValidator shopGroupFromInputValidator,
            IShopGroupRepository shopGroupRepository, IGroupIdProvider groupIdProvider)
        {
            _usersApi = usersApi;
            _groupMemberRepository = groupMemberRepository;
            _tokenResolver = tokenResolver;
            _groupMemberResolver = groupMemberResolver;
            _shopGroupFromInputValidator = shopGroupFromInputValidator;
            _platformTokenProvider = platformTokenProvider;
            _shopGroupRepository = shopGroupRepository;
            _groupIdProvider = groupIdProvider;

        }
        public ActionResult Index(long userId, string token)
        {
            _platformTokenProvider.Set(token);
           if (!_tokenResolver.IsTokenResolvedForUser(userId))
                return Redirect("/home");

            var model = GetModel();
            model.Status = JoinStatus.NewAction;
            return View(model);
        }

        public ActionResult JoinGroup()
        {
            var model = GetModel();
            if (!_tokenResolver.IsTokenResolvedForUser())
            {
                model.Status = JoinStatus.FailGeneral;
                return View("Join", model);
            }
            var groupId = _groupIdProvider.Get();
            model.Status = groupId <= 0 ? JoinStatus.FailGeneral : _groupMemberResolver.JoinUserToGroup(groupId);        
            
            return View("Join",model);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Join(FormCollection formValue)
        {
            if (!_shopGroupFromInputValidator.IsInputValid(formValue, new InputFiled[] { InputFiled.Token, InputFiled.GroupId, InputFiled.UserId }))
                return View();

            var tokenStr = _shopGroupFromInputValidator.GetStringKey(InputFiled.Token, formValue);
            _platformTokenProvider.Set(tokenStr);

            var userId = _shopGroupFromInputValidator.GetLongKey(InputFiled.UserId, formValue);
            if (!_tokenResolver.IsTokenResolvedForUser(userId))
                return Redirect("/home");

            var groupId = _shopGroupFromInputValidator.GetLongKey(InputFiled.GroupId, formValue);
            
            var status = _groupMemberResolver.JoinUserToGroup(userId, (int)groupId);
            var model = GetModel((int)groupId);
            model.Status = status;
            if (model.Status == JoinStatus.Success)
                model.GroupName = _shopGroupRepository.GetShoupGroup((int) groupId).Name;

            return View(model);
        }

        private JoinGroupScreenModel GetModel(int groupId = 0)
        {
            var currentUser = _usersApi.Current();
            var groupMember = _groupMemberRepository.GroupMember(currentUser.Id);

            return new JoinGroupScreenModel
            {
                GroupId = groupId,
                Token = _platformTokenProvider.Get(),
                MemberId = currentUser.Id,
                MemberName = currentUser.Name,
                ShopGroups = groupMember != null ?
                     groupMember.Groups :
                        null
            };
        }

    }
}