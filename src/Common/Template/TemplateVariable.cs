using System;

namespace AstralKeks.Workbench.Common.Template
{
    public partial struct TemplateVariable
    {

        public TemplateVariable(string name, string value = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Variable name is not provided", nameof(name));

            Name = string.Format(TemplateFormat.VariableNameFormat, name);
            Value = value ?? string.Empty;
        }

        public string Name { get; }

        public string Value { get; }
        
        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
