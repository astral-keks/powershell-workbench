using Bogus;
using System.IO;

namespace AstralKeks.Workbench.Utilities
{
    internal static class FakerSystemExtensions
    {
        public static string PSFilePath(this Bogus.DataSets.System system, char separator)
        {
            var drive = Path.GetFileNameWithoutExtension(new Faker().System.FileName());
            var path = $"/{system.FilePath()}".Replace('/', separator);
            return $"{drive}:{path}";
        }
    }
}
