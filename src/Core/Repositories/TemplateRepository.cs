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
        private readonly SharedContext _sharedContext;

        public TemplateRepository(FileSystem fileSystem, SharedContext sharedContext)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
        }

        public IEnumerable<string> GetTemplates()
        {
            var workspaceTemplateDirectory = PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory, Directories.Template);
            var userspaceTemplateDirectory = PathBuilder.Combine(_sharedContext.CurrentUserspaceDirectory, Directories.Template);

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
