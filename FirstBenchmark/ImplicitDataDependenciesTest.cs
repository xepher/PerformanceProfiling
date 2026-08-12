using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace FirstBenchmark;

[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class ImplicitDataDependenciesTest
{
    private double[] a = new double[100];

    [Benchmark(Baseline = true)]
    public double Loop()
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++)
            sum += a[i];
        return sum;
    }

    [Benchmark]
    public double UnrolledLoop()
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i += 4)
            sum += a[i] + a[i + 1] + a[i + 2] + a[i + 3];
        return sum;
    }
}