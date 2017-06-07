using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace AstralKeks.Workbench.PowerShell.Parameters
{
    internal class DynamicParameterCompleter : IArgumentCompleter
    {
        private static readonly Dictionary<string, Func<string, object>> _completers = new Dictionary<string, Func<string, object>>();
        private static readonly Dictionary<string, DynamicParameterContainer> _containers = new Dictionary<string, DynamicParameterContainer>();

        public static void RegisterCompleter(string commandName, string parameterName, Func<string, object> func)
        {
            _completers[$"{commandName}.{parameterName}"] = func;
        }

        public static void RegisterContainer(string commandName, DynamicParameterContainer container)
        {
            _containers[commandName] = container;
        }

        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            object result = null;
            IDictionary buffer = new Dictionary<string, object>();

            try
            {
                if (_containers.ContainsKey(commandName))
                    FixParameterValues(_containers[commandName], fakeBoundParameters, buffer);

                if (_completers.ContainsKey($"{commandName}.{parameterName}"))
                    result = _completers[$"{commandName}.{parameterName}"].Invoke(wordToComplete);
            }
            finally
            {
                if (_containers.ContainsKey(commandName))
                    FixParameterValues(_containers[commandName], buffer);
            }

            if (result is IEnumerable<string>)
            {
                foreach (var completionEntry in result as IEnumerable<string>)
                {
                    yield return new CompletionResult(completionEntry);
                }
            }
        }

        private void FixParameterValues(DynamicParameterContainer destination, IDictionary source, IDictionary buffer = null)
        {
            foreach (string parameterName in source.Keys)
            {
                var parameterValue = source[parameterName];
                if (parameterValue != null)
                {
                    var parameter = destination.Get(parameterName);
                    if (parameter != null)
                    {
                        if (buffer != null)
                            buffer[parameter.Name] = parameter.Value;
                        parameter.Value = parameterValue;
                    }
                }
            }
        }
    }
}
