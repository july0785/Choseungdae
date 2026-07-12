using System.IO;
using ChosonTyping.Core;

namespace ChosonTyping.Tests;

public class WordBankTests
{
    static KeyboardLayout Kukgyu() =>
        KeyboardLayout.Load(Path.Combine(AppContext.BaseDirectory, "data", "layouts", "kukgyu.json"));

    [Fact]
    public void 점수는_글쇠수를_센다()
    {
        var l = Kukgyu();
        // 안 = ㅇㅏㄴ = 3글쇠, 가 = ㄱㅏ = 2글쇠
        Assert.Equal(3, WordBank.Score("안", l));
        Assert.Equal(2, WordBank.Score("가", l));
    }

    [Fact]
    public void 윗글쇠_자모는_값을_더_먹는다()
    {
        var l = Kukgyu();
        // 까 = ㄲ(윗글쇠 S) + ㅏ = 2글쇠 + 윗글쇠 1 = 3
        Assert.True(WordBank.Score("까", l) > WordBank.Score("가", l));
    }

    [Fact]
    public void 긴_낱말이_짧은_낱말보다_높은_단계()
    {
        var l = Kukgyu();
        var words = new[] { "가", "나", "다", "라", "마", "안녕하십니까", "조선민주주의" };
        var bank = new WordBank(words, l, new Random(1));
        Assert.True(bank.TierOf("안녕하십니까") > bank.TierOf("가"));
    }

    [Fact]
    public void 단계별로_다섯칸에_고루_들어간다()
    {
        var l = Kukgyu();
        var words = Enumerable.Range(0, 50).Select(i => new string('가', 1 + i % 8)).Distinct().ToArray();
        var bank = new WordBank(words, l, new Random(1));
        foreach (var w in words)
            Assert.InRange(bank.TierOf(w), 1, 5);
    }

    [Fact]
    public void 게임단계가_오르면_뽑기_중심이_높은_단계로()
    {
        var l = Kukgyu();
        var words = Enumerable.Range(1, 40).Select(i => new string('가', 1 + i % 8)).ToArray();
        var bank = new WordBank(words, l, new Random(1));
        var low = bank.Weights(1);   // 1단계
        var high = bank.Weights(9);  // 9단계
        Assert.True(low[0] > low[4]);   // 낮은 게임단계 → 낮은 낱말단계 무게가 큼
        Assert.True(high[4] > high[0]); // 높은 게임단계 → 높은 낱말단계 무게가 큼
    }
}
