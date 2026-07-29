using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace FirstBenchmark;

// we should test mono4.9 and mono5.2
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class ArrayCountBenchmark
{
    private readonly IList<object> array = new string[0];

    [Benchmark]
    public int CountProblem() => array.Count;
}
