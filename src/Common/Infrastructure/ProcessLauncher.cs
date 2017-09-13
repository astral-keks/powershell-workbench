using System.Diagnostics;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class ProcessLauncher
    {
        public void Launch(ProcessStartInfo processInfo, bool waitForExit)
        {
            var process = Process.Start(processInfo);
            if (waitForExit)
                process.WaitForExit();
        }
    }
}
