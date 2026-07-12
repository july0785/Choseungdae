namespace ChosonTyping.Core;

/// <summary>
/// 낱말을 치기 어려운 정도(글쇠수 + 윗글쇠 부담)로 5단계로 가른다.
/// 산성비에서 단계가 오를수록 높은 단계 낱말이 자주 나오게 뽑아준다.
/// </summary>
public sealed class WordBank
{
    readonly List<string>[] _tiers = { new(), new(), new(), new(), new() };
    readonly Random _rng;

    public int Count { get; }

    public WordBank(IEnumerable<string> words, KeyboardLayout layout, Random rng)
    {
        _rng = rng;
        var scored = words.Select(w => (w, s: Score(w, layout)))
                          .OrderBy(x => x.s).ThenBy(x => x.w, StringComparer.Ordinal).ToList();
        Count = scored.Count;
        for (int i = 0; i < scored.Count; i++)
        {
            int tier = scored.Count <= 1 ? 0 : Math.Min(4, i * 5 / scored.Count);
            _tiers[tier].Add(scored[i].w);
        }
    }

    /// <summary>치기 어려운 정도: 글쇠수 + 윗글쇠 글쇠수(윗글쇠는 한번 더 값).</summary>
    public static int Score(string word, KeyboardLayout layout)
    {
        var plan = KeystrokePlanner.PlanText(word, layout);
        int units = plan.Sum(u => u.Count);
        int shift = plan.Sum(u => u.Count(k => k.Shift));
        return units + shift;
    }

    /// <summary>낱말의 단계(1~5). 없는 낱말이면 0.</summary>
    public int TierOf(string word)
    {
        for (int t = 0; t < 5; t++)
            if (_tiers[t].Contains(word)) return t + 1;
        return 0;
    }

    /// <summary>단계별 뽑기 무게 — 게임 단계가 오를수록 높은 낱말단계로 중심이 옮겨간다.</summary>
    public double[] Weights(int gameLevel)
    {
        double focus = Math.Clamp(1 + (gameLevel - 1) * 0.5, 1, 5); // 1단계→1, 9단계→5
        var w = new double[5];
        for (int t = 0; t < 5; t++)
            w[t] = _tiers[t].Count == 0 ? 0 : Math.Max(0.12, 1.0 - Math.Abs((t + 1) - focus) * 0.45);
        return w;
    }

    /// <summary>게임 단계에 맞춰 낱말 하나 뽑기.</summary>
    public string Pick(int gameLevel)
    {
        var w = Weights(gameLevel);
        double total = w.Sum();
        if (total <= 0)
        {
            var all = _tiers.SelectMany(t => t).ToList();
            return all[_rng.Next(all.Count)];
        }
        double r = _rng.NextDouble() * total;
        int tier = 4;
        for (int t = 0; t < 5; t++)
        {
            if (w[t] <= 0) continue;
            r -= w[t];
            if (r <= 0) { tier = t; break; }
        }
        var list = _tiers[tier].Count > 0 ? _tiers[tier] : _tiers.First(x => x.Count > 0);
        return list[_rng.Next(list.Count)];
    }
}
