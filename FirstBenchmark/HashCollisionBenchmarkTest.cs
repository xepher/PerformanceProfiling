using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace FirstBenchmark;

/// <summary>
/// 模拟未启用 FEATURE_RANDOMIZED_STRING_HASHING 时的传统确定性哈希比较器 (如经典 DJB2 或多项式 Hash)
/// </summary>
public class LegacyDeterministicStringComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y) => string.Equals(x, y);

    public int GetHashCode(string obj)
    {
        unchecked
        {
            int hash = 0;
            foreach (char c in obj)
            {
                hash = hash * 31 + c;
            }
            return hash;
        }
    }
}

[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class HashCollisionBenchmarkTest
{
    private List<string> _collidingStrings = null!;
    private List<string> _normalStrings = null!;

    [Params(100, 500, 1000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        // 生成具有 100% 相同哈希值的碰撞字符串列表（基于 "Aa" 与 "BB" 在确定性多项式 Hash 下的等价碰撞特性）
        _collidingStrings = GenerateCollidingStrings(Count);

        // 生成正常分布的字符串列表
        _normalStrings = Enumerable.Range(0, Count)
                                   .Select(i => $"Normal_Item_{i:D6}")
                                   .ToList();
    }

    /// <summary>
    /// 构造指定数量的哈希碰撞字符串 (生成 > 100 个具有完全相同 HashCode 的不同字符串)
    /// </summary>
    public static List<string> GenerateCollidingStrings(int count)
    {
        var list = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            var sb = new StringBuilder();
            // 10 组 "Aa"/"BB" 组合可生成 2^10 = 1024 个碰撞字符串
            for (int bit = 0; bit < 10; bit++)
            {
                sb.Append((i & (1 << bit)) != 0 ? "BB" : "Aa");
            }
            list.Add(sb.ToString());
        }
        return list;
    }

    /// <summary>
    /// 测试在传统确定性哈希下，发生 Hash 碰撞攻击时 HashSet 插入退化为 O(N^2) 的性能
    /// </summary>
    [Benchmark]
    public HashSet<string> LegacyHashingWithCollisions()
    {
        var set = new HashSet<string>(new LegacyDeterministicStringComparer());
        foreach (var str in _collidingStrings)
        {
            set.Add(str);
        }
        return set;
    }

    /// <summary>
    /// 测试在现代随机化哈希 / 正常分布数据下，HashSet 维持 O(N) 的标准性能
    /// </summary>
    [Benchmark]
    public HashSet<string> ModernHashingNormal()
    {
        var set = new HashSet<string>();
        foreach (var str in _normalStrings)
        {
            set.Add(str);
        }
        return set;
    }
}
