using AstralKeks.PowerShell.Common.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace AstralKeks.Workbench.Sandbox
{
    public class SourceControlItem : CmdletProviderItem
    {
        public SourceControlItem(string path) : base(path.Replace("src:", "D:\\Workplace"))
        {
        }

        public override bool CanCreate() => true;

        public override bool CanGet() => true;

        public override bool CanSet() => true;

        public override bool CanRead() => true;

        public override bool CanWrite() => true;

        public override bool CanClear() => true;

        public override bool CanInvoke() => true;

        public override bool CanRemove() => true;

        public override bool CanList() => Directory.Exists(Path);

        public override bool CanMove(string destinationPath) => true;

        public override bool CanCopy(string destinationPath) => true;


        public override bool Validate()
        {
            try
            {
                System.IO.Path.GetFullPath(Path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Exists()
        {
            return Directory.Exists(Path) || File.Exists(Path);
        }

        public override object Get()
        {
            return new FileInfo(Path);
        }

        public override object Clear()
        {
            File.WriteAllText(Path, string.Empty);
            return new FileInfo(Path);
        }

        public override object Invoke()
        {
            return Process.Start(Path);
        }

        public override object Remove(bool recurse = false)
        {
            File.Delete(Path);
            return new FileInfo(Path);
        }

        public override object Move(string destinationPath)
        {
            File.Move(Path, destinationPath);
            return new FileInfo(Path);
        }

        public override object Copy(string destinationPath, bool recurse = false)
        {
            File.Copy(Path, destinationPath);
            return new FileInfo(Path);
        }

        public override void Create(object value)
        {
            new FileInfo(Path).Create();
        }

        public override void Set(object value, bool append = false)
        {
            new FileInfo(Path).Create();
        }

        public override IEnumerable<string> List(bool recurse = false, uint depth = uint.MaxValue)
        {
            return Directory.EnumerateFileSystemEntries(Path).Select(e => e.Replace("D:\\Workplace", "src:"));
        }

        public override Stream Read()
        {
            return File.OpenRead(Path);
        }

        public override Stream Write()
        {
            return File.OpenWrite(Path);
        }
    }
}
