// _ Summary _

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)

AMD Ryzen 5 5600G with Radeon Graphics 3.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.302

[Host] : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3

DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3

| Method     |     Mean |   Error |  StdDev |
| ---------- | -------: | ------: | ------: |
| Newtonsoft | 254.0 us | 4.99 us | 5.94 us |
| SystemText | 102.2 us | 1.87 us | 2.80 us |
