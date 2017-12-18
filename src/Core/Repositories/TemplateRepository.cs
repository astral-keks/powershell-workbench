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
        private readonly SessionContext _sessionContext;

        public TemplateRepository(FileSystem fileSystem, SessionContext sessionContext)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
        }

        public IEnumerable<string> GetTemplates()
        {
            var workspaceTemplateDirectory = PathBuilder.Complete(_sessionContext.CurrentWorkspaceDirectory, Directories.Template);
            var userspaceTemplateDirectory = PathBuilder.Combine(_sessionContext.CurrentUserspaceDirectory, Directories.Template);

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
