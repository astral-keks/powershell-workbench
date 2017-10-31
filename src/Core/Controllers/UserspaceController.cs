using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using System;

namespace AstralKeks.Workbench.Controllers
{
    public class UserspaceController
    {
        private readonly UserspaceContext _userspaceContext;
        private readonly UserspaceRepository _userspaceRepository;

        public UserspaceController(UserspaceContext userspaceContext, UserspaceRepository userspaceRepository)
        {
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _userspaceRepository = userspaceRepository ?? throw new ArgumentNullException(nameof(userspaceRepository));
        }

        public Userspace EnterUserspace(string userspaceName, Func<Userspace> onMissing, Func<Userspace> onFallback)
        {
            var defaultUserspace = _userspaceRepository.GetUserspace(userspaceDirectory: _userspaceContext.DefaultUserspaceDirectory);
            _userspaceRepository.CreateUserspace(defaultUserspace);

            Userspace userspace;
            if (!string.IsNullOrWhiteSpace(userspaceName))
            {
                userspace = _userspaceRepository.FindUserspace(userspaceName: userspaceName);
                if (userspace == null)
                    userspace = onMissing();
                if (userspace == null)
                    userspace = onFallback();
            }
            else
                userspace = _userspaceRepository.FindUserspace(userspaceDirectory: _userspaceContext.RecentUserspaceDirectory);
            
            return EnterUserspace(userspace);
        }

        public Userspace EnterUserspace(Userspace userspace = null)
        {
            if (userspace != null)
            {
                _userspaceContext.CurrentUserspaceDirectory = userspace.Directory;
                _userspaceContext.RecentUserspaceDirectory = userspace.Directory;
            }

            return userspace;
        }

        public Userspace ExitUserspace()
        {
            var userspace = _userspaceRepository.GetUserspace(null, _userspaceContext.CurrentUserspaceDirectory);
            _userspaceContext.CurrentUserspaceDirectory = null;

            return userspace;
        }
    }
}
