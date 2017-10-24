using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command.Template
{
    [Cmdlet(VerbsCommon.Format, Noun.WBTemplate)]
    public class FormatTemplateCmdlet : WorkbenchCmdlet, IDynamicParameters
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string TemplatePath { get; set; }

        public RuntimeDefinedParameterDictionary TemplateParameters { get; set; }

        public object GetDynamicParameters()
        {
            if (TemplateParameters == null)
                TemplateParameters = new RuntimeDefinedParameterDictionary();

            var templateVariables = Components.TemplateController.GetTemplateVariables(TemplatePath);
            foreach (var variable in templateVariables)
                TemplateParameters[variable] = new RuntimeDefinedParameter(variable, typeof(string), new Collection<Attribute>());

            return TemplateParameters;
        }

        protected override void ProcessRecord()
        {
            var templateVariables = TemplateParameters.Values.ToDictionary(p => p.Name, p => p.Value);
            var resultPath = Components.TemplateController.FormatTemplate(TemplatePath, templateVariables);
            WriteObject(resultPath);
        }
    }
}
