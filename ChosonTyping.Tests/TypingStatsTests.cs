using ChosonTyping.Core;

namespace ChosonTyping.Tests;

public class TypingStatsTests
{
    [Fact]
    public void 타속은_타수_나누기_분() =>
        Assert.Equal(300, TypingStats.Cpm(150, TimeSpan.FromSeconds(30)));

    [Fact]
    public void 시간이_0이면_타속도_0() =>
        Assert.Equal(0, TypingStats.Cpm(10, TimeSpan.Zero));

    [Fact]
    public void 정확도는_마지막_상태_기준() =>
        Assert.Equal(75, TypingStats.Accuracy("아버지가", "아버지도"));

    [Fact]
    public void 정확도_전부_맞으면_100() =>
        Assert.Equal(100, TypingStats.Accuracy("맑다", "맑다"));

    [Fact]
    public void 안_친_자리는_틀린것으로_치지_않는다() =>
        Assert.Equal(100, TypingStats.Accuracy("아버지", "아"));

    [Fact]
    public void 아무것도_안_쳤으면_100() =>
        Assert.Equal(100, TypingStats.Accuracy("아버지", ""));

    [Fact]
    public void 진행률() =>
        Assert.Equal(50, TypingStats.Progress("아버지가", "아버"));
}
