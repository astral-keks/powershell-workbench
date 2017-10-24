using System.Collections.Generic;

namespace AstralKeks.Workbench.Common.Template
{
    public partial class TemplateModel : List<TemplateVariable>
    {
        public void Add(string name, string value) => Add(new TemplateVariable(name, value));
    }
}
