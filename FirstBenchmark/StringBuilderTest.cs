using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Text;

namespace FirstBenchmark;

[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class StringBuilderTest
{
    private List<string> _stringList = null!;

    [Params(1_000, 5_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _stringList = Enumerable.Range(1, Count)
                                .Select(i => $"Item_{i:D4}")
                                .ToList();
    }

    [Benchmark]
    public string BuildString()
    {
        string buffer = string.Empty;
        foreach (var str in _stringList)
        {
            buffer += str;
        }
        return buffer;
    }

    [Benchmark]
    public string BuildStringWithStringBuilder()
    {
        var buffer = new StringBuilder();
        foreach (var str in _stringList)
        {
            buffer.Append(str);
        }
        return buffer.ToString();
    }
}
