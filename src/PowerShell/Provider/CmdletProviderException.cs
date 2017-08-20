using System;

namespace AstralKeks.Workbench.PowerShell.Provider
{
    [Serializable]
    public class CmdletProviderException : Exception
    {
        public CmdletProviderException() { }
        public CmdletProviderException(string message) : base(message) { }
        public CmdletProviderException(string message, Exception inner) : base(message, inner) { }
        protected CmdletProviderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UnsupportedCmdletProviderOperationException : CmdletProviderException
    {
        public UnsupportedCmdletProviderOperationException() : base("This operation is not supported") { }
        public UnsupportedCmdletProviderOperationException(Exception inner) : base("This operation is not supported", inner) { }
        protected UnsupportedCmdletProviderOperationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ItemExistsCmdletProviderException : CmdletProviderException
    {
        public ItemExistsCmdletProviderException() : base("Item already exists") { }
        public ItemExistsCmdletProviderException(Exception inner) : base("Item already exists", inner) { }
        protected ItemExistsCmdletProviderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
