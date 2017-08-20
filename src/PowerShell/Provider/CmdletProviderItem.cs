using System.Collections.Generic;
using System.IO;

namespace AstralKeks.Workbench.PowerShell.Provider
{
    public class CmdletProviderItem
    {
        public CmdletProviderItem(string path)
        {
            Path = path;
        }

        public virtual string Path { get; }

        public virtual bool CanCreate() => true;

        public virtual bool CanGet() => true;

        public virtual bool CanSet() => true;

        public virtual bool CanRead() => true;

        public virtual bool CanWrite() => true;

        public virtual bool CanClear() => true;

        public virtual bool CanInvoke() => true;

        public virtual bool CanRemove() => true;

        public virtual bool CanList() => false;

        public virtual bool CanMove(string destinationPath) => false;

        public virtual bool CanCopy(string destinationPath) => false;


        public virtual bool Validate() => true;

        public virtual bool Exists() => false;

        public virtual object Get() => null;

        public virtual object Clear() => null;

        public virtual object Invoke() => null;

        public virtual object Remove(bool recurse = false) => null;

        public virtual object Move(string destinationPath) => null;

        public virtual object Copy(string destinationPath, bool recurse = false) => null;

        public virtual Stream Read() => Stream.Null;

        public virtual Stream Write() => Stream.Null;

        public virtual void Create(object value) { }

        public virtual void Set(object value, bool append = false) { }

        public virtual IEnumerable<string> List(bool recurse = false, uint depth = uint.MaxValue) { yield break; }

        public virtual object this[string propertyName]
        {
            get { return null; }
            set { }
        }
    }
}
