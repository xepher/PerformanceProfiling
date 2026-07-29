using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace FirstBenchmark;

/// <summary>
/// 测试 100-Case 超大 Switch 语句在真正的 Legacy C# 编译器与 Roslyn 编译器的性能差异
/// 
/// 编译器来源：
/// 1. Real Legacy C# Compiler:
///    调用 Windows 原生 Pre-Roslyn C# 5 编译器 (C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe) 
///    动态编译 100-Case Switch 源码 (符合 C# 5 语法) 并加载程序集。
/// 
/// 2. Modern Roslyn Compiler:
///    使用当前 .NET 8/9/10 内置的现代 Roslyn C# 编译器直接编译执行 100-Case Switch 语句。
/// </summary>
[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class HugeSwitchBenchmarkTest
{
    private static Func<string, int>? _legacyCscDelegate;

    private List<string> _testInputs = null!;

    [Params(100, 1000)]
    public int Iterations { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        EnsureLegacyCscCompiled();

        // 校验算法正确性 (TDD 单元验证)
        ValidateCorrectness();

        var rawInputs = Enumerable.Range(0, 100).Select(i => $"Command{i:D3}").ToList();
        rawInputs.Add("UnknownCommand");

        _testInputs = new List<string>(Iterations);
        for (int i = 0; i < Iterations; i++)
        {
            _testInputs.Add(rawInputs[i % rawInputs.Count]);
        }
    }

    private static void EnsureLegacyCscCompiled()
    {
        if (_legacyCscDelegate == null)
        {
            _legacyCscDelegate = CompileAndLoadLegacyCscAssembly();
        }
    }

    /// <summary>
    /// TDD 校验方法：验证真实 Roslyn 编译出的 Switch 与 Legacy csc.exe 编译出的 Switch 结果绝对一致
    /// </summary>
    public static void ValidateCorrectness()
    {
        EnsureLegacyCscCompiled();

        var func = _legacyCscDelegate!;

        for (int i = 0; i < 100; i++)
        {
            string cmd = $"Command{i:D3}";
            int roslynResult = ExecuteRoslynSwitch(cmd);
            int legacyResult = func(cmd);

            if (roslynResult != i || legacyResult != i)
            {
                throw new InvalidOperationException($"Switch result mismatch for input '{cmd}'. Roslyn: {roslynResult}, Legacy: {legacyResult}, Expected: {i}");
            }
        }

        // 测试 default / 未匹配分支
        string unknown = "NonExistentCommand";
        if (ExecuteRoslynSwitch(unknown) != -1 || func(unknown) != -1)
        {
            throw new InvalidOperationException("Default case mismatch for unknown input.");
        }
    }

    /// <summary>
    /// 真正的现代 Roslyn 编译器（.NET 8/9/10 内嵌）编译的 100 Case switch 语句
    /// </summary>
    [Benchmark(Baseline = true)]
    public int ModernRoslynCompilerBenchmark()
    {
        int sum = 0;
        foreach (var input in _testInputs)
        {
            sum += ExecuteRoslynSwitch(input);
        }
        return sum;
    }

    /// <summary>
    /// 真正的原生 Legacy csc.exe (C# 5.0 pre-Roslyn) 编译的 100 Case switch 语句
    /// </summary>
    [Benchmark]
    public int RealLegacyCscCompilerBenchmark()
    {
        int sum = 0;
        var func = _legacyCscDelegate!;
        foreach (var input in _testInputs)
        {
            sum += func(input);
        }
        return sum;
    }

    /// <summary>
    /// Roslyn 编译器直接编译的 100 Case switch
    /// 可以通过 https://sharplab.io 在线查看 SyntaxFactory C# 调用代码
    /// </summary>
    public static int ExecuteRoslynSwitch(string input)
    {
        switch (input)
        {
            case "Command000": return 0;
            case "Command001": return 1;
            case "Command002": return 2;
            case "Command003": return 3;
            case "Command004": return 4;
            case "Command005": return 5;
            case "Command006": return 6;
            case "Command007": return 7;
            case "Command008": return 8;
            case "Command009": return 9;
            case "Command010": return 10;
            case "Command011": return 11;
            case "Command012": return 12;
            case "Command013": return 13;
            case "Command014": return 14;
            case "Command015": return 15;
            case "Command016": return 16;
            case "Command017": return 17;
            case "Command018": return 18;
            case "Command019": return 19;
            case "Command020": return 20;
            case "Command021": return 21;
            case "Command022": return 22;
            case "Command023": return 23;
            case "Command024": return 24;
            case "Command025": return 25;
            case "Command026": return 26;
            case "Command027": return 27;
            case "Command028": return 28;
            case "Command029": return 29;
            case "Command030": return 30;
            case "Command031": return 31;
            case "Command032": return 32;
            case "Command033": return 33;
            case "Command034": return 34;
            case "Command035": return 35;
            case "Command036": return 36;
            case "Command037": return 37;
            case "Command038": return 38;
            case "Command039": return 39;
            case "Command040": return 40;
            case "Command041": return 41;
            case "Command042": return 42;
            case "Command043": return 43;
            case "Command044": return 44;
            case "Command045": return 45;
            case "Command046": return 46;
            case "Command047": return 47;
            case "Command048": return 48;
            case "Command049": return 49;
            case "Command050": return 50;
            case "Command051": return 51;
            case "Command052": return 52;
            case "Command053": return 53;
            case "Command054": return 54;
            case "Command055": return 55;
            case "Command056": return 56;
            case "Command057": return 57;
            case "Command058": return 58;
            case "Command059": return 59;
            case "Command060": return 60;
            case "Command061": return 61;
            case "Command062": return 62;
            case "Command063": return 63;
            case "Command064": return 64;
            case "Command065": return 65;
            case "Command066": return 66;
            case "Command067": return 67;
            case "Command068": return 68;
            case "Command069": return 69;
            case "Command070": return 70;
            case "Command071": return 71;
            case "Command072": return 72;
            case "Command073": return 73;
            case "Command074": return 74;
            case "Command075": return 75;
            case "Command076": return 76;
            case "Command077": return 77;
            case "Command078": return 78;
            case "Command079": return 79;
            case "Command080": return 80;
            case "Command081": return 81;
            case "Command082": return 82;
            case "Command083": return 83;
            case "Command084": return 84;
            case "Command085": return 85;
            case "Command086": return 86;
            case "Command087": return 87;
            case "Command088": return 88;
            case "Command089": return 89;
            case "Command090": return 90;
            case "Command091": return 91;
            case "Command092": return 92;
            case "Command093": return 93;
            case "Command094": return 94;
            case "Command095": return 95;
            case "Command096": return 96;
            case "Command097": return 97;
            case "Command098": return 98;
            case "Command099": return 99;
            default: return -1;
        }
    }

    /// <summary>
    /// 调用 Windows 原生 C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe 
    /// 动态编译相同源码 (严格遵循 C# 5 语法，如使用标准 Block Namespace)，生成真实的 Legacy 编译器程序集
    /// </summary>
    private static Func<string, int> CompileAndLoadLegacyCscAssembly()
    {
        string legacyCscPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";
        if (!File.Exists(legacyCscPath))
        {
            throw new FileNotFoundException($"Legacy C# compiler not found at path: {legacyCscPath}");
        }

        string tempDir = Path.Combine(Path.GetTempPath(), "LegacyCscBenchmark");
        Directory.CreateDirectory(tempDir);

        string sourcePath = Path.Combine(tempDir, "LegacySwitchContainer.cs");
        string dllPath = Path.Combine(tempDir, "LegacySwitchContainer.dll");

        // 必须使用 C# 5 兼容语法（标准大括号 Block Namespace，不能使用 C# 10 的 File-scoped namespace）
        var codeBuilder = new System.Text.StringBuilder();
        codeBuilder.AppendLine("namespace LegacyCompiled");
        codeBuilder.AppendLine("{");
        codeBuilder.AppendLine("    public static class LegacySwitchContainer");
        codeBuilder.AppendLine("    {");
        codeBuilder.AppendLine("        public static int ExecuteSwitch(string input)");
        codeBuilder.AppendLine("        {");
        codeBuilder.AppendLine("            switch (input)");
        codeBuilder.AppendLine("            {");
        for (int i = 0; i < 100; i++)
        {
            codeBuilder.AppendLine($"                case \"Command{i:D3}\": return {i};");
        }
        codeBuilder.AppendLine("                default: return -1;");
        codeBuilder.AppendLine("            }");
        codeBuilder.AppendLine("        }");
        codeBuilder.AppendLine("    }");
        codeBuilder.AppendLine("}");

        File.WriteAllText(sourcePath, codeBuilder.ToString());

        var psi = new ProcessStartInfo
        {
            FileName = legacyCscPath,
            Arguments = $"/target:library /out:\"{dllPath}\" \"{sourcePath}\"",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(psi);
        process?.WaitForExit();

        if (process?.ExitCode != 0 || !File.Exists(dllPath))
        {
            string err = process?.StandardError.ReadToEnd() ?? "Unknown compilation error";
            throw new InvalidOperationException($"Legacy csc.exe failed with exit code {process?.ExitCode}: {err}");
        }

        byte[] assemblyBytes = File.ReadAllBytes(dllPath);
        Assembly asm = Assembly.Load(assemblyBytes);
        Type type = asm.GetType("LegacyCompiled.LegacySwitchContainer")!;
        MethodInfo method = type.GetMethod("ExecuteSwitch", BindingFlags.Public | BindingFlags.Static)!;
        return (Func<string, int>)Delegate.CreateDelegate(typeof(Func<string, int>), method);
    }
}
