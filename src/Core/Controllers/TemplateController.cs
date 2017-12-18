using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Template;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Controllers
{
    public class TemplateController
    {
        private readonly FileSystem _fileSystem;
        private readonly SessionContext _sessionContext;
        private readonly TemplateProcessor _templateProcessor;

        public TemplateController(SessionContext sessionContext, FileSystem fileSystem, TemplateProcessor templateProcessor)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }

        public IEnumerable<string> GetTemplateVariables(string templatePath)
        {
            var variables = Enumerable.Empty<string>();

            if (!string.IsNullOrWhiteSpace(templatePath))
            {
                var templateContent = _fileSystem.FileReadText(templatePath);
                variables = _templateProcessor.Explore(templateContent);
            }

            return variables;
        }

        public string FormatTemplate(string templatePath, Dictionary<string, object> templateVariables)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
                throw new ArgumentException("Template path is not set", nameof(templatePath));
            if (templateVariables == null)
                throw new ArgumentNullException(nameof(templateVariables));

            var templateModel = TemplateModel.Dictionary(templateVariables);
            var templateContent = _fileSystem.FileReadText(templatePath);
            templateContent = _templateProcessor.Transform(templateContent, templateModel);

            var resultFolder = _sessionContext.CurrentWorkspaceDirectory ?? _sessionContext.CurrentUserspaceDirectory;
            var resultFileNameBase = Path.GetFileNameWithoutExtension(templatePath);
            var resultFileNameDate = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fffffff");
            var resultFileName = $"{resultFileNameBase}-{resultFileNameDate}{Path.GetExtension(templatePath)}";
            var resultPath = PathBuilder.Combine(resultFolder, Directories.Temp, Directories.Workbench, Directories.Template, resultFileName);
            _fileSystem.FileCreate(resultPath, templateContent);

            return resultPath;
        }
    }
}
