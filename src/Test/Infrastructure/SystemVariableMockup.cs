using System;
using System.Collections;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Infrastructure
{
    public class SystemVariableMockup : SystemVariable
    {
        private readonly Dictionary<EnvironmentVariableTarget, Hashtable> _storages;

        public SystemVariableMockup()
        {
            _storages = new Dictionary<EnvironmentVariableTarget, Hashtable>
            {
                { EnvironmentVariableTarget.Machine, new Hashtable() },
                { EnvironmentVariableTarget.User, new Hashtable() },
                { EnvironmentVariableTarget.Process, new Hashtable() },
            };
        }

        public string HomeUniversal
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(LocalAppData))
                    SetVariable("LOCALAPPDATA", value);
                else
                    SetVariable("HOME", value);
            }
        }

        public void AddVariable(string name, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            SetVariable(name, value, target);
        }

        public override IDictionary GetVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            return _storages[target];
        }

        protected override string GetVariable(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            return _storages[target].ContainsKey(name) ? (string)_storages[target][name] : null;
        }

        protected override void SetVariable(string name, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            _storages[target][name] = value;
        }
    }
}
