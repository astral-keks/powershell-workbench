namespace AstralKeks.Workbench.Common.Content
{
    public interface IResourceWriter
    {
        bool CanWrite(string resourceName);

        void Write(string resourceName, string resourceContent);
    }
}
