using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace FirstBenchmark;

[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class DataDependenciesTest
{
    private int n = 1000001;
    private double x0, x1, x2, x3, x4, x5, x6, x7;

    [Benchmark(Baseline = true)]
    public void WithoutDependencies()
    {
        for (int i = 0; i < n; i++)
        {
            x0++;
            x1++;
            x2++;
            x3++;
            x4++;
            x5++;
            x6++;
            x7++;
        }
    }

    [Benchmark]
    public void WithDependencies()
    {
        for (int i = 0; i < n; i++)
        {
            x0++;
            x0++;
            x0++;
            x0++;
            x0++;
            x0++;
            x0++;
        }
    }
}