using System;

namespace AstralKeks.Workbench.PowerShell.Attributes
{
    public class DynamicCompleterAttribute : Attribute
    {
        public DynamicCompleterAttribute(string completerFunctionName)
        {
            if (string.IsNullOrWhiteSpace(completerFunctionName))
                throw new ArgumentNullException(nameof(completerFunctionName));

            CompleterFunctionName = completerFunctionName;
        }

        public string CompleterFunctionName { get; set; }
    }
}
