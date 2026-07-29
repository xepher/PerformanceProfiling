using BenchmarkDotNet.Running;
using FirstBenchmark;

// Validate HugeSwitch logic before benchmark runner
HugeSwitchBenchmarkTest.ValidateCorrectness();
Console.WriteLine("HugeSwitchBenchmarkTest correctness validation passed!");

BenchmarkRunner.Run<HugeSwitchBenchmarkTest>();