using AstralKeks.Workbench.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    internal static class SessionOriginator
    {
        public static void Update(this SessionState cmdletSession, Func<Userspace> userspaceProvider)
        {
            var userspace = userspaceProvider?.Invoke();
            if (userspace != null)
            {
                cmdletSession.Import(userspace.Profile);
                cmdletSession.Execute($"Update-WBUserspaceSession");
            }
        }

        public static void Restore(this SessionState cmdletSession, Func<Userspace> userspaceProvider)
        {
            var userspace = userspaceProvider?.Invoke();
            if (userspace != null)
            {
                cmdletSession.Import(userspace.Profile);
                cmdletSession.Execute($"Restore-WBUserspaceSession");
            }
        }

        public static void Update(this SessionState cmdletSession, Func<Workspace> workspaceProvider)
        {
            var memento = new SessionMemento(Directory.GetCurrentDirectory(), cmdletSession.GetLocation());
            SessionMemento.Save(memento);

            var workspace = workspaceProvider?.Invoke();
            if (workspace != null)
            {
                cmdletSession.Import(workspace.Profile);

                cmdletSession.Relocate(workspace.Directory, workspace.Directory);
                cmdletSession.Execute($"Update-WBWorkspaceSession");
            }
        }

        public static void Restore(this SessionState cmdletSession, Func<Workspace> workspaceProvider)
        {
            var memento = SessionMemento.Load();

            var workspace = workspaceProvider?.Invoke();
            if (workspace != null)
            {
                cmdletSession.Import(workspace.Profile);

                cmdletSession.Execute($"Restore-WBWorkspaceSession");
                cmdletSession.Relocate(memento?.Directory, memento?.Location);
            }
        }
        
        private static void Relocate(this SessionState cmdletSession, string directory, string location)
        {
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.SetCurrentDirectory(directory);
            if (!string.IsNullOrWhiteSpace(location))
                cmdletSession.SetLocation(location);
        }

        private static void Import(this SessionState cmdletSession, string profile)
        {
            if (!string.IsNullOrWhiteSpace(profile) && File.Exists(profile))
                cmdletSession.Execute($"& '{profile}'");
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

        private static Collection<PSObject> Execute(this SessionState cmdletSession, string script)
        {
            try
            {
                return cmdletSession.InvokeCommand.InvokeScript(script);
            }
            catch (CommandNotFoundException)
            {
                return new Collection<PSObject>();
            }
        }
    }
}
