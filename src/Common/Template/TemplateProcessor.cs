using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Common.Template
{
    public class TemplateProcessor
    {
        private readonly FileSystem _fileSystem;
        private readonly SystemVariable _systemVariable;

        public TemplateProcessor(FileSystem fileSystem, SystemVariable systemVariable)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public IEnumerable<string> Explore(string template)
        {
            var variableRegex = string.Format(TemplateFormat.VariableNameFormat, "placeholder");
            variableRegex = Regex.Escape(variableRegex).Replace("placeholder", $"([^{TemplateFormat.VariableEnding}]+)");

            var matches = Regex.Matches(template, variableRegex);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    if (match.Groups[1].Success)
                        yield return match.Groups[1].Value;
                }
            }
        }

        public string Transform(string template, TemplateModel model = null)
        {
            if (!string.IsNullOrWhiteSpace(template))
            {
                model = model ?? new TemplateModel();
                model.AddRange(TemplateModel.Default(_fileSystem, _systemVariable));

                foreach (var variable in model)
                    template = template.Replace(variable.Name, variable.Value); 
            }

            return template;
        }

        public IResource Transform(IResource template, TemplateModel model = null)
        {
            if (template != null)
            {
                template = new Resource(template.Name, template.Content);
                if (template.CanRead && template.CanWrite)
                {
                    model = model ?? new TemplateModel();
                    model.AddRange(TemplateModel.Default(_fileSystem, _systemVariable));

                    foreach (var variable in model)
                    {
                        var value = JsonConvert.ToString(variable.Value);
                        value = value.Substring(1, value.Length - 2);
                        template.Content = template.Content.Replace(variable.Name, value);
                    }
                }
            }

            return template;
        }
    }
}
