using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command.Template
{
    [Cmdlet(VerbsDiagnostic.Resolve, Noun.WBTemplate)]
    [OutputType(typeof(string))]
    public class ResolveTemplateCmdlet : WorkbenchCmdlet, IDynamicParameters
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string TemplateShortcutQuery { get; set; }

        public RuntimeDefinedParameterDictionary TemplateParameters { get; set; }

        protected override void ProcessRecord()
        {
            var shortcuts = Components.ShortcutController.FindShortcut(TemplateShortcutQuery).ToList();
            if (shortcuts.Count > 1)
                throw new ArgumentException($"More than 1 shortcut found for query {TemplateShortcutQuery}");

            var templatePath = shortcuts.Single().Target;
            var templateVariables = TemplateParameters.Values.ToDictionary(p => p.Name, p => p.Value);
            var resultPath = Components.TemplateController.FormatTemplate(templatePath, templateVariables);
            WriteObject(resultPath);
        }

        public object GetDynamicParameters()
        {
            if (TemplateParameters == null)
                TemplateParameters = new RuntimeDefinedParameterDictionary();

            var shortcuts = Components.ShortcutController.FindShortcut(TemplateShortcutQuery).ToList();
            if (shortcuts.Count == 1)
            {
                var templatePath = shortcuts.Single().Target;
                var templateVariables = Components.TemplateController.GetTemplateVariables(templatePath);
                foreach (var variable in templateVariables)
                    TemplateParameters[variable] = new RuntimeDefinedParameter(variable, typeof(string), new Collection<Attribute>
                    {
                        new ParameterAttribute()
                    });
            }

            return TemplateParameters;
        }

        [ParameterCompleter(nameof(TemplateShortcutQuery))]
        public IEnumerable<string> CompleteTemplateQuery(string queryPart)
        {
            return Components.ShortcutController.FindShortcut(queryPart).Select(s => s.ToString());
        }
    }
}
