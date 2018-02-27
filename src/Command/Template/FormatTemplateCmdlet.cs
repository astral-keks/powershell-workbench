using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command.Template
{
    [Cmdlet(VerbsCommon.Format, Noun.WBTemplate)]
    [OutputType(new[] { typeof(string) })]
    public class FormatTemplateCmdlet : WorkbenchPSCmdlet, IDynamicParameters
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string TemplatePath { get; set; }

        public RuntimeDefinedParameterDictionary TemplateParameters { get; set; }

        protected override void ProcessRecord()
        {
            var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(TemplatePath, out ProviderInfo provider);
            foreach (var resolvedPath in resolvedPaths)
            {
                var templateVariables = TemplateParameters.Values.ToDictionary(p => p.Name, p => p.Value);
                var resultPath = Components.TemplateController.FormatTemplate(resolvedPath, templateVariables);
                WriteObject(resultPath);
            }
        }

        public object GetDynamicParameters()
        {
            if (TemplateParameters == null)
                TemplateParameters = new RuntimeDefinedParameterDictionary();

            var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(TemplatePath, out ProviderInfo provider);
            foreach (var resolvedPath in resolvedPaths)
            {
                var templateVariables = Components.TemplateController.GetTemplateVariables(TemplatePath);
                foreach (var variable in templateVariables)
                    TemplateParameters[variable] = new RuntimeDefinedParameter(variable, typeof(string), new Collection<Attribute>());
            }

            return TemplateParameters;
        }
    }
}
