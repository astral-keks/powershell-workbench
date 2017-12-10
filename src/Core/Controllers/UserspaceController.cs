using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using System;
using System.Linq;

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

        public bool CheckUserspace(string userspaceName, Func<bool> shouldCreate)
        {
            var userspace = GetUserspace(userspaceName);
            if (userspace == null && shouldCreate())
                userspace = _userspaceRepository.CreateUserspace(userspaceName);

            return userspace != null;
        }

        public Userspace UseUserspace(string userspaceName)
        {
            var userspace = GetUserspace(userspaceName);
            if (userspace != null)
                _userspaceContext.CurrentUserspaceDirectory = userspace.Directory;

            return userspace;
        }

        private Userspace GetUserspace(string userspaceName = null)
        {
            if (string.IsNullOrWhiteSpace(userspaceName))
                userspaceName = _userspaceRepository.DefineUserspace(null, _userspaceContext.DefaultUserspaceDirectory).Name;

            return _userspaceRepository.GetUserspaces().FirstOrDefault(u => u.Name.Is(userspaceName));
        }
    }
}
