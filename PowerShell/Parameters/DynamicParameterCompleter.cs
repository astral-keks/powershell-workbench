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

        public static void RegisterCompleter(string commandName, string parameterName, Func<string, object> func)
        {
            _completers[$"{commandName}.{parameterName}"] = func;
        }
        
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            object result = null;

            if (_completers.ContainsKey($"{commandName}.{parameterName}"))
                result = _completers[$"{commandName}.{parameterName}"].Invoke(wordToComplete);

            if (result is IEnumerable<string>)
            {
                foreach (var completionEntry in result as IEnumerable<string>)
                {
                    yield return new CompletionResult(completionEntry);
                }
            }
        }

    }
}
