﻿using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using AstralKeks.Workbench.PowerShell.Attributes;

namespace AstralKeks.Workbench.PowerShell.Parameters
{
    internal class DynamicParameterBuilder
    {
        private readonly Cmdlet _cmdlet;

        public DynamicParameterBuilder(Cmdlet cmdlet)
        {
            _cmdlet = cmdlet;
        }

        public void Build(DynamicParameterContainer container)
        {
            var parameters = _cmdlet.GetType().GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttributes(typeof(DynamicParameterAttribute), true)
                        .Cast<DynamicParameterAttribute>()
                        .SingleOrDefault()
                })
                .Where(p => p.Attribute != null)
                .Select(p => BuildParameter(_cmdlet, p.Property, p.Attribute));

            foreach (var parameter in parameters)
                container.Add(parameter.Name, parameter);
        }

        private RuntimeDefinedParameter BuildParameter(Cmdlet cmdlet, PropertyInfo property, DynamicParameterAttribute attribute)
        {
            var parameter = new RuntimeDefinedParameter
            {
                Name = property.Name,
                ParameterType = property.PropertyType
            };

            var parameterAttribute = BuildParameterAttribute(attribute);
            if (parameterAttribute != null)
                parameter.Attributes.Add(parameterAttribute);

            var validateSetAttribute = BuildValidateSetAttribute(cmdlet, property);
            if (validateSetAttribute != null)
                parameter.Attributes.Add(validateSetAttribute);

            var argumentCompleterAttribute = BuildArgumentCompleterAttribute(cmdlet, property);
            if (argumentCompleterAttribute != null)
                parameter.Attributes.Add(argumentCompleterAttribute);

            return parameter;
        }

        private ParameterAttribute BuildParameterAttribute(DynamicParameterAttribute attribute)
        {
            ParameterAttribute parameterAttribute = null;
            if (attribute.Position >= 0)
            {
                parameterAttribute = new ParameterAttribute();
                parameterAttribute.Position = attribute.Position;
                parameterAttribute.Mandatory = attribute.Mandatory;
                parameterAttribute.ValueFromPipeline = attribute.ValueFromPipeline;
                parameterAttribute.ParameterSetName = attribute.ParameterSetName;
            }

            return parameterAttribute;
        }

        private ValidateSetAttribute BuildValidateSetAttribute(Cmdlet cmdlet, PropertyInfo property)
        {
            ValidateSetAttribute validateSetAttribute = null;
            
            var validateDynamicSetAttribute = property.GetCustomAttribute<ValidateDynamicSetAttribute>();
            if (validateDynamicSetAttribute != null)
            {
                var valuesProviderName = validateDynamicSetAttribute.ValuesFunctionName;
                var valuesProviderHolder = property.DeclaringType;
                var valuesProvider = valuesProviderHolder?.GetMethods().FirstOrDefault(m => m.Name == valuesProviderName);
                if (valuesProvider != null /*&& valuesProvider.ReturnType.IsSubclassOf(typeof(Array))*/)
                {
                    var command = cmdlet.CommandRuntime.ToString();
                    var parameter = property.Name;
                    var arguments = valuesProvider.GetParameters().Length == 2
                        ? new object[] { command, parameter }
                        : new object[0];
                    var values = (Array)valuesProvider.Invoke(cmdlet, arguments);
                    if (values.Length > 0)
                        validateSetAttribute = new ValidateSetAttribute(values.Cast<string>().ToArray());
                }
            }

            return validateSetAttribute;
        }

        private ArgumentCompleterAttribute BuildArgumentCompleterAttribute(Cmdlet cmdlet, PropertyInfo property)
        {
            ArgumentCompleterAttribute argumentCompleterAttribute = null;

            var dynamicCompleterAttribute = property.GetCustomAttribute<DynamicCompleterAttribute>();
            if (dynamicCompleterAttribute != null)
            {
                var completerFunctionName = dynamicCompleterAttribute.CompleterFunctionName;
                var completerFunctionHolder = property.DeclaringType;
                var completerFunction = completerFunctionHolder?.GetMethods().FirstOrDefault(m => m.Name == completerFunctionName);
                if (completerFunction != null)
                {
                    var command = cmdlet.CommandRuntime.ToString();
                    var parameter = property.Name;
                    var arguments = completerFunction.GetParameters().Length ==  3
                        ? new object[] { command, parameter }
                        : new object[0];
                    DynamicParameterCompleter.RegisterCompleter(command, parameter,
                        w => completerFunction.Invoke(cmdlet, arguments.Concat(new object[] {w}).ToArray()));

                    argumentCompleterAttribute = new ArgumentCompleterAttribute(typeof(DynamicParameterCompleter));
                }
            }

            return argumentCompleterAttribute;
        }
    }
}
