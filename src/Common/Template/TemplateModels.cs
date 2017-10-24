using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Common.Template
{
    public partial class TemplateModel
    {
        public static TemplateModel Default(FileSystem fileSystem, SystemVariable systemVariable)
        {
            return new TemplateModel
            {
                TemplateVariable.Bin(fileSystem.BinDirectory()),
                TemplateVariable.User(systemVariable.UserDirectory),
                TemplateVariable.Userspace(systemVariable.UserspaceDirectory),
                TemplateVariable.Workspace(systemVariable.WorkspaceDirectory)
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

        public static TemplateModel Dictionary(Dictionary<string, object> source)
        {
            var model = new TemplateModel();

            foreach (var variableSource in source)
                model.Add(variableSource.Key, variableSource.Value?.ToString() ?? string.Empty);

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
