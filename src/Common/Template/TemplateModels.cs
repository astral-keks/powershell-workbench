using AstralKeks.Workbench.Common.Context;
using System.Collections;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Common.Template
{
    public partial class TemplateModel
    {
        public static TemplateModel Default(GlobalContext globalContext, SessionContext sessionContext)
        {
            return new TemplateModel
            {
                TemplateVariable.Workbench(globalContext.ApplicationDirectory),
                TemplateVariable.Userspace(sessionContext.CurrentUserspaceDirectory),
                TemplateVariable.Workspace(sessionContext.CurrentWorkspaceDirectory)
            };
        }

        public static TemplateModel Object(object source)
        {
            var model = new TemplateModel();

            var type = source.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(source)?.ToString();

                model.Add(propertyName, propertyValue);
            }

            return model;
        }

        public static TemplateModel Dictionary(IDictionary source)
        {
            var model = new TemplateModel();

            foreach (DictionaryEntry variableSource in source)
                model.Add(variableSource.Key.ToString(), variableSource.Value?.ToString() ?? string.Empty);

            return model;
        }

        public static TemplateModel PipelineAndArgs(string pipeline, IEnumerable<string> args)
        {
            return new TemplateModel
            {
                TemplateVariable.Pipeline(pipeline),
                TemplateVariable.Args(args)
            };
        }
    }
}
