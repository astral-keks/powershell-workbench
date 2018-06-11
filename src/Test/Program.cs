using System.Reflection;

namespace AstralKeks.Workbench
{
    class Program
    {
        static void Main(string[] args)
        {
            var testAssembly = Assembly.GetAssembly(typeof(Program)).Location;
            Xunit.ConsoleClient.Program.Main(new[] { testAssembly });
        }
    }
}
