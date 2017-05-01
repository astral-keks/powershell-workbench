using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class Workspace
    {
        public Workspace(string[] directories, WorkspaceFile[] files)
        {
            Directories = directories ?? throw new ArgumentNullException(nameof(directories));
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }

        public string[] Directories { get; }

        public WorkspaceFile[] Files { get; }
    }

    public class WorkspaceFile
    {
        public string Directory { get; set; }
        public string Filename { get; set; }
    }
}
