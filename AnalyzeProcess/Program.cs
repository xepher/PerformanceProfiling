// https://github.com/microsoft/clrmd
// Microsoft.Diagnostics.Runtime is a set of APIs for introspecting processes and dumps.
using Microsoft.Diagnostics.Runtime;
using System.Diagnostics;

namespace AnalyzeProcess;

class Program
{
    const string TargetProcessName = "../LargeMemoryUsage/bin/Release/net10.0/LargeMemoryUsage.exe";

    static void Main(string[] args)
    {
        var startInfo = new ProcessStartInfo(TargetProcessName);
        startInfo.CreateNoWindow = true;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        var targetProcess = Process.Start(startInfo);
        Thread.Sleep(1000);
        using (DataTarget target = DataTarget.AttachToProcess(
            targetProcess.Id,
            suspend: true))
        {
            PrintDumpInfo(target);

            var clr = target.ClrVersions[0].CreateRuntime();
        }
    }
    private static void PrintDumpInfo(DataTarget target)
    {
        PrintHeader("Target Info");

        Console.WriteLine($"Architecture: {target.DataReader.Architecture}");
        Console.WriteLine($"Pointer Size: {target.DataReader.PointerSize}");
        Console.WriteLine("CLR Versions:");
        foreach (var clr in target.ClrVersions)
        {
            Console.WriteLine($"\t{clr.Version}");
        }
    }

    private static void PrintHeader(string value)
    {
        Console.WriteLine();
        Console.WriteLine(value);
        Console.WriteLine(new string('=', value.Length));
    }
}