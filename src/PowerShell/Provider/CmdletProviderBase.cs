using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace AstralKeks.Workbench.PowerShell.Provider
{
    public abstract class CmdletProviderBase : NavigationCmdletProvider, IContentCmdletProvider, IPropertyCmdletProvider
    {
        protected virtual IEnumerable<PSDriveInfo> OnInit()
        {
            yield break;
        }

        protected virtual CmdletProviderItem OnItem(string path)
        {
            return new CmdletProviderItem(path);
        }

        protected virtual void OnOutput(CmdletProviderItem item, object value = null)
        {
            if (item != null)
            {
                var path = item.Path;
                var isContainer = item.CanList();
                if (value == null && item.CanGet())
                    value = item.Get();

                if (value != null)
                    WriteItemObject(value, path, isContainer);
            }
        }

        protected virtual void OnPropertyOutput(CmdletProviderItem item, object propertyValue)
        {
            if (item != null && propertyValue != null)
            {
                WritePropertyObject(propertyValue, item.Path);
            }
        }

        #region DriveCmdletProvider
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var drives = OnInit().ToList();
            return new Collection<PSDriveInfo>(drives);
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            return base.NewDrive(drive);
        }

        protected override object NewDriveDynamicParameters()
        {
            return base.NewDriveDynamicParameters();
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            return base.RemoveDrive(drive);
        }
        #endregion

        #region ItemCmdletProvider
        protected override void ClearItem(string path)
        {
            var item = OnItem(path);
            if (item.CanClear())
            {
                var value = item.Clear();
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            return base.ClearItemDynamicParameters(path);
        }

        protected override string[] ExpandPath(string path)
        {
            var expandedPaths = base.ExpandPath(path);
            return expandedPaths;
        }

        protected override void GetItem(string path)
        {
            var item = OnItem(path);
            if (item.CanGet())
            {
                var value = item.Get();
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object GetItemDynamicParameters(string path)
        {
            return base.GetItemDynamicParameters(path);
        }

        protected override void InvokeDefaultAction(string path)
        {
            var item = OnItem(path);
            if (item.CanInvoke())
            {
                var value = item.Invoke();
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return base.InvokeDefaultActionDynamicParameters(path);
        }

        protected override bool IsValidPath(string path)
        {
            var item = OnItem(path);
            return item.Validate();
        }

        protected override bool ItemExists(string path)
        {
            var item = OnItem(path);
            return item.Exists();
        }

        protected override object ItemExistsDynamicParameters(string path)
        {
            return base.ItemExistsDynamicParameters(path);
        }

        protected override void SetItem(string path, object value)
        {
            var item = OnItem(path);
            if (item.Exists())
            {
                if (item.CanSet())
                {
                    item.Set(value);
                    OnOutput(item, value);
                }
                else
                    throw new UnsupportedCmdletProviderOperationException();
            }
            else
            {
                if (item.CanCreate())
                {
                    item.Create(value);
                    OnOutput(item, value);
                }
                else
                    throw new UnsupportedCmdletProviderOperationException();
            }
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return base.SetItemDynamicParameters(path, value);
        }

        #endregion

        #region ContainerCmdletProvider

        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            var success = base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
            return success;
        }

        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            var item = OnItem(path);
            if (item.CanCopy(copyPath))
            {
                var value = item.Copy(copyPath, recurse);
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            return base.CopyItemDynamicParameters(path, destination, recurse);
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            var item = OnItem(path);
            if (item.CanList())
            {
                foreach (var childItem in item.List(recurse).Select(OnItem))
                    OnOutput(childItem);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            var item = OnItem(path);
            if (item.CanList())
            {
                foreach (var childItem in item.List(recurse, depth).Select(OnItem))
                    OnOutput(childItem);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            return base.GetChildItemsDynamicParameters(path, recurse);
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            var item = OnItem(path);
            if (item.CanList())
            {
                foreach (var childItemPath in item.List())
                {
                    var childName = GetChildName(childItemPath);
                    OnOutput(item, childName);
                }
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return base.GetChildNamesDynamicParameters(path);
        }

        protected override bool HasChildItems(string path)
        {
            var item = OnItem(path);
            return item.CanList() && item.List().Any();
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            var item = OnItem(path);
            if (!item.Exists())
            {
                if (item.CanCreate())
                {
                    item.Create(newItemValue);
                    OnOutput(item, newItemValue);
                }
                else
                    throw new UnsupportedCmdletProviderOperationException();
            }
            else
                throw new ItemExistsCmdletProviderException();
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return base.NewItemDynamicParameters(path, itemTypeName, newItemValue);
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            var item = OnItem(path);
            if (item.CanRemove())
            {
                var value = item.Remove(recurse);
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return base.RemoveItemDynamicParameters(path, recurse);
        }

        protected override void RenameItem(string path, string newName)
        {
            var item = OnItem(path);
            if (item.CanMove(newName))
            {
                var value = item.Move(newName);
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return base.RenameItemDynamicParameters(path, newName);
        }
        #endregion

        #region NavigationCmdletProvider
        protected override string GetChildName(string path)
        {
            var childName = base.GetChildName(path);
            return childName;
        }

        protected override string GetParentPath(string path, string root)
        {
            var parentPath = base.GetParentPath(path, root);
            return parentPath;
        }

        protected override bool IsItemContainer(string path)
        {
            var item = OnItem(path);
            return item.CanList();
        }

        protected override string MakePath(string parent, string child)
        {
            var path = base.MakePath(parent, child);
            return path;
        }

        protected override void MoveItem(string path, string destination)
        {
            var item = OnItem(path);
            if (item.CanMove(destination))
            {
                var value = item.Move(destination);
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return base.MoveItemDynamicParameters(path, destination);
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return base.NormalizeRelativePath(path, basePath);
        }
        #endregion

        #region ContentCmdletProvider
        public void ClearContent(string path)
        {
            var item = OnItem(path);
            if (item.CanClear())
            {
                var value = item.Clear();
                OnOutput(item, value);
            }
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        public object ClearContentDynamicParameters(string path)
        {
            return null;
        }

        public IContentReader GetContentReader(string path)
        {
            var item = OnItem(path);
            if (item.CanRead())
                return new CmdletProviderReader(item);
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            return null;
        }

        public IContentWriter GetContentWriter(string path)
        {
            var item = OnItem(path);
            if (item.CanWrite())
                return new CmdletProviderWriter(item);
            else
                throw new UnsupportedCmdletProviderOperationException();
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            return null;
        }
        #endregion

        #region PropertyCmdletProvider
        public void ClearProperty(string path, Collection<string> propertyToClear)
        {
            var item = OnItem(path);
            foreach (var property in propertyToClear)
                item[property] = null;
        }

        public object ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear)
        {
            return null;
        }

        public void GetProperty(string path, Collection<string> providerSpecificPickList)
        {
            var item = OnItem(path);
            foreach (var propertyName in providerSpecificPickList)
            {
                var propetyValue = item[propertyName];
                OnPropertyOutput(item, propetyValue);
            }
        }

        public object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList)
        {
            return null;
        }

        public void SetProperty(string path, PSObject property)
        {
            var item = OnItem(path);
            foreach (var propertyInfo in property.Properties)
                item[propertyInfo.Name] = propertyInfo.Value;
        }

        public object SetPropertyDynamicParameters(string path, PSObject propertyValue)
        {
            return null;
        } 
        #endregion
    }
}
