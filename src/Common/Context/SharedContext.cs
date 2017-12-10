﻿using AstralKeks.Workbench.Common.Infrastructure;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class SharedContext
    {
        private readonly FileSystem _fileSystem;
        private readonly SystemVariable _systemVariable;
        private readonly GlobalContext _globalContext;
        private readonly WorkspaceContext _workspaceContext;
        private readonly UserspaceContext _userspaceContext;

        public SharedContext(FileSystem fileSystem, SystemVariable systemVariable,
            GlobalContext globalContext, WorkspaceContext workspaceContext, UserspaceContext userspaceContext)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
        }

        public bool IsBound => !string.IsNullOrWhiteSpace(_userspaceContext.CurrentUserspaceDirectory);

        public string CurrentWorkspaceDirectory => IsBound
            ? _workspaceContext.CurrentWorkspaceDirectory
            : _fileSystem.DirectoryGetCurrent();

        public string CurrentUserspaceDirectory => IsBound
            ? _userspaceContext.CurrentUserspaceDirectory
            : _globalContext.AltUserDirectory;
    }
}