﻿using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Repositories;
using Autofac;
using System;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class ResourceBootstrapper : IStartable
    {
        private readonly UserspaceContext _userspaceContext;
        private readonly WorkspaceContext _workspaceContext;
        private readonly UserspaceRepository _userspaceRepository;
        private readonly WorkspaceRepository _workspaceRepository;

        public ResourceBootstrapper(UserspaceContext userspaceContext, WorkspaceContext workspaceContext,
            UserspaceRepository userspaceRepository, WorkspaceRepository workspaceRepository)
        {
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceRepository = userspaceRepository ?? throw new ArgumentNullException(nameof(userspaceRepository));
            _workspaceRepository = workspaceRepository ?? throw new ArgumentNullException(nameof(workspaceRepository));
        }

        public void Start()
        {
            var defaultUserspaceDirectory = _userspaceContext.DefaultUserspaceDirectory;
            var currentUserspaceDirectory = _userspaceContext.CurrentUserspaceDirectory;
            var currentWorkspaceDirectory = _workspaceContext.CurrentWorkspaceDirectory;

            BootstrapUserspace(defaultUserspaceDirectory);
            if (!string.Equals(currentUserspaceDirectory, defaultUserspaceDirectory, StringComparison.OrdinalIgnoreCase))
                BootstrapUserspace(currentUserspaceDirectory);
            if (!string.IsNullOrWhiteSpace(currentWorkspaceDirectory))
                BootstrapWorkspace(currentWorkspaceDirectory);
        }
        
        protected virtual void BootstrapUserspace(string userspaceDirectory)
        {
            var userspace = _userspaceRepository.GetUserspace(userspaceDirectory: userspaceDirectory);
            _userspaceRepository.CreateUserspace(userspace);
        }

        protected virtual void BootstrapWorkspace(string workspaceDirectory)
        {
            var workspace = _workspaceRepository.GetWorkspace(workspaceDirectory);
            _workspaceRepository.CreateWorkspace(workspace);
        }
    }
}
