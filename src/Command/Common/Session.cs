using AstralKeks.Workbench.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    internal static class Session
    {
        public static void Update(this SessionState cmdletSession, Func<Userspace> userspaceProvider)
        {
            var userspace = userspaceProvider?.Invoke();
            if (userspace != null)
            {
                foreach (var profile in userspace.Profiles)
                    cmdletSession.Import(profile);
            };
        }

        public static void Update(this SessionState cmdletSession, Func<Workspace> workspaceProvider)
        {
            var workspace = workspaceProvider?.Invoke();
            if (workspace != null)
            {
                cmdletSession.Relocate(workspace.Directory, workspace.Directory);
                foreach (var profile in workspace.Profiles)
                    cmdletSession.Import(profile);
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
