﻿using System;
using System.Collections;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class SystemVariable
    {
        public string WorkspaceDirectory
        {
            get { return GetVariable("WBWorkspaceDirectory"); }
            set { SetVariable("WBWorkspaceDirectory", value); }
        }

        public string UserspaceDirectory
        {
            get { return GetVariable("WBUserspaceDirectory"); }
            set { SetVariable("WBUserspaceDirectory", value); }
        }

        public string LocalAppData
        {
            get { return GetVariable("LOCALAPPDATA"); }
        }

        public string Home
        {
            get { return GetVariable("HOME"); }
        }

        public virtual IDictionary GetVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            return target == EnvironmentVariableTarget.Process
                ? Environment.GetEnvironmentVariables()
                : Environment.GetEnvironmentVariables(target);
        }

        public virtual string GetVariable(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            return target == EnvironmentVariableTarget.Process
                ? Environment.GetEnvironmentVariable(name)
                : Environment.GetEnvironmentVariable(name, target);
        }

        public virtual void SetVariable(string name, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            Environment.SetEnvironmentVariable(name, value, target);
        }
    }
}
