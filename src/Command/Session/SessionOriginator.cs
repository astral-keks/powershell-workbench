using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AstralKeks.Workbench.Command
{
    internal static class SessionOriginator
    {
        public static void Restore(this SessionState cmdletSession, Action restoreCallback)
        {
            try
            {
                restoreCallback();
            }
            finally
            {
                cmdletSession.Restore();
            }
        }

        public static void Update(this SessionState cmdletSession, Action updateCallback)
        {
            cmdletSession.Save();
            try
            {
                updateCallback();
            }
            finally
            {
                cmdletSession.Save();
            }
        }

        public static void Update(this SessionState cmdletSession, string sessionProfile, string location = null)
        {
            if (!string.IsNullOrWhiteSpace(sessionProfile) && File.Exists(sessionProfile))
                cmdletSession.InvokeCommand.InvokeScript($"& '{sessionProfile}'");
            if (!string.IsNullOrWhiteSpace(location))
                cmdletSession.InvokeCommand.InvokeScript($"Set-Location {location}");
        }

        public static void Save(this SessionState cmdletSession)
        {
            var directory = Directory.GetCurrentDirectory();
            var location = cmdletSession.GetLocation();
            var modulePath = cmdletSession.GetModulePath();
            var modules = cmdletSession.GetModules();
            var aliases = cmdletSession.GetAliases();

            var memento = new SessionMemento(directory, location, modulePath, modules, aliases);
            SessionMemento.Saved.Enqueue(memento);
        }

        public static void Restore(this SessionState cmdletSession)
        {
            if (SessionMemento.Saved.Count == 2)
            {
                var primaryMemento = SessionMemento.Saved.Dequeue();
                var secondaryMemento = SessionMemento.Saved.Dequeue();
                var removingModules = secondaryMemento.Modules.Except(primaryMemento.Modules).ToList();
                var addingModules = primaryMemento.Modules.Except(secondaryMemento.Modules).ToList();
                var removingAliases = secondaryMemento.Aliases.Except(primaryMemento.Aliases).ToList();
                var addingAliases = primaryMemento.Aliases.Except(secondaryMemento.Aliases).ToList();

                Directory.SetCurrentDirectory(primaryMemento.Directory);
                cmdletSession.SetLocation(primaryMemento.Location);
                cmdletSession.SetModulePath(primaryMemento.ModulePath);
                cmdletSession.RemoveModules(removingModules);
                cmdletSession.ImportModules(addingModules);
                cmdletSession.RemoveAliases(removingAliases);
                cmdletSession.SetAliases(addingAliases);
            }

            SessionMemento.Saved.Clear();
        }


        private static string GetLocation(this SessionState cmdletSession)
        {
            var psObjects = cmdletSession.Execute("Get-Location");
            var pathInfo = (PathInfo)psObjects.Single().BaseObject;
            return pathInfo.Path;
        }

        private static void SetLocation(this SessionState cmdletSession, string location)
        {
            cmdletSession.Execute($"Set-Location {location}");
        }

        private static string GetModulePath(this SessionState cmdletSession)
        {
            var psObjects = cmdletSession.Execute("$env:PSModulePath");
            return (string)psObjects.Single().BaseObject;
        }

        private static void SetModulePath(this SessionState cmdletSession, string modulePath)
        {
            cmdletSession.Execute($"$env:PSModulePath = '{modulePath}'");
        }

        private static List<PSModuleInfo> GetModules(this SessionState cmdletSession)
        {
            var psObjects = cmdletSession.Execute("Get-Module");
            return psObjects.Select(o => (PSModuleInfo)o.BaseObject).ToList();
        }

        private static void RemoveModules(this SessionState cmdletSession, List<PSModuleInfo> modules)
        {
            foreach (var module in modules)
                cmdletSession.Execute("Remove-Module -ModuleInfo $args[0]", module);
        }

        private static void ImportModules(this SessionState cmdletSession, List<PSModuleInfo> modules)
        {
            foreach (var module in modules)
                cmdletSession.Execute("Import-Module -ModuleInfo $args[0] -Scope Global", module);
        }

        private static List<AliasInfo> GetAliases(this SessionState cmdletSession)
        {
            var psObjects = cmdletSession.Execute("Get-Alias");
            return psObjects.Select(o => (AliasInfo)o.BaseObject).ToList();
        }

        private static void RemoveAliases(this SessionState cmdletSession, List<AliasInfo> aliases)
        {
            foreach (var alias in aliases)
                cmdletSession.Execute($"Remove-Item alias:\\{alias.Name}");
        }

        private static void SetAliases(this SessionState cmdletSession, List<AliasInfo> aliases)
        {
            foreach (var alias in aliases)
                cmdletSession.Execute($"Set-Item {alias.Name} {alias.Definition} -Scope Global");
        }

        private static Collection<PSObject> Execute(this SessionState cmdletSession, string script)
        {
            return cmdletSession.InvokeCommand.InvokeScript(script);
        }

        private static Collection<PSObject> Execute(this SessionState cmdletSession, string script, params object[] args)
        {
            return cmdletSession.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null, args);
        }
    }
}
