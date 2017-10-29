using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Repositories
{
    public class TemplateRepository
    {
        private readonly FileSystem _fileSystem;
        private readonly WorkspaceContext _workspaceContext;
        private readonly UserspaceContext _userspaceContext;

        public TemplateRepository(FileSystem fileSystem, WorkspaceContext workspaceContext, UserspaceContext userspaceContext)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
        }

        public IEnumerable<string> GetTemplates()
        {
            var workspaceTemplateDirectory = PathBuilder.Complete(_workspaceContext.CurrentWorkspaceDirectory, Directories.Template);
            var userspaceTemplateDirectory = PathBuilder.Combine(_userspaceContext.CurrentUserspaceDirectory, Directories.Template);

            if (string.IsNullOrWhiteSpace(workspaceTemplateDirectory))
            {
                foreach (var templatePath in _fileSystem.DirectoryList(workspaceTemplateDirectory))
                    yield return templatePath;
            }

            foreach (var templatePath in _fileSystem.DirectoryList(userspaceTemplateDirectory))
                yield return templatePath;
        }
    }
}
