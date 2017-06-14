using System;
using System.Linq;

namespace AstralKeks.Workbench.Launcher
{
    internal class Prompt
    {
        public static bool YesNo(string message)
        {
            string answer = null;
            string[] answers = new[] { "y", "n" };
            do
            {
                Console.WriteLine(message);
                Console.Write("[y] yes  [n] no (default is \"yes\"): ");
            }
            while (!answers.Contains(answer = Console.ReadLine()?.Trim().ToLower()));

            return answer == "y";
        }
    }
}
