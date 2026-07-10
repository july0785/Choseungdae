using ChosonTyping.Core;

namespace ChosonTyping.Tests;

public class HangulComposerTests
{
    static HangulComposer Typed(string jamos)
    {
        var c = new HangulComposer();
        foreach (var j in jamos) c.PutJamo(j);
        return c;
    }

    [Theory]
    [InlineData("ㅇㅏㄴ", "안")]
    [InlineData("ㅈㅗㅅㅓㄴ", "조선")]        // 도깨비불: 좃+ㅓ → 조서 → 조선
    [InlineData("ㄱㅏㅂㅅ", "값")]            // 겹받침 ㅄ
    [InlineData("ㄱㅏㅂㅅㅏ", "갑사")]        // 겹받침 도깨비불
    [InlineData("ㄱㅗㅏ", "과")]              // 두 타 겹모음
    [InlineData("ㅁㅜㅓ", "뭐")]
    [InlineData("ㅇㅡㅣ", "의")]
    [InlineData("ㅇㅓㅂㅅ", "없")]
    [InlineData("ㅃㅏ", "빠")]                // 쌍자음 초성
    [InlineData("ㄱㅏㅆ", "갔")]              // 쌍자음 받침
    [InlineData("ㅏㄴㅣ", "ㅏ니")]            // 초성 없는 모음은 그대로
    [InlineData("ㄱㄴ", "ㄱㄴ")]              // 자음 연속은 각각
    [InlineData("ㄱㅏㄸ", "가ㄸ")]            // ㄸ은 받침 불가
    [InlineData("ㄱㅗㅏㅣ", "과ㅣ")]          // 겹모음 뒤 모음은 새로
    [InlineData("ㄷㅏㄹㄱ", "닭")]            // 겹받침 ㄺ
    [InlineData("ㅇㅏㄴㄴㅕㅇ", "안녕")]
    public void 모아쓰기(string input, string expected) =>
        Assert.Equal(expected, Typed(input).Text);

    [Fact]
    public void 창덕_통짜_겹모음() =>
        Assert.Equal("과", Typed("ㄱㅘ").Text);

    [Fact]
    public void 사이띄기는_조합을_끝낸다()
    {
        var c = Typed("ㄱㅏ");
        c.PutText(' ');
        foreach (var j in "ㄴㅏ") c.PutJamo(j);
        Assert.Equal("가 나", c.Text);
    }

    [Fact]
    public void 백스페이스_자모단위()
    {
        var c = Typed("ㅇㅏㄴ");             // 안
        Assert.True(c.Backspace());
        Assert.Equal("아", c.Text);
        Assert.True(c.Backspace());
        Assert.Equal("ㅇ", c.Text);
        Assert.True(c.Backspace());
        Assert.Equal("", c.Text);
        Assert.False(c.Backspace());
    }

    [Fact]
    public void 백스페이스_두타_겹모음은_한타만_되돌린다()
    {
        var c = Typed("ㄱㅗㅏ");             // 과
        c.Backspace();
        Assert.Equal("고", c.Text);
    }

    [Fact]
    public void 백스페이스_창덕_통짜는_통째로()
    {
        var c = Typed("ㄱㅘ");               // 과 (ㅘ가 한 타)
        c.Backspace();
        Assert.Equal("ㄱ", c.Text);
    }

    [Fact]
    public void 백스페이스_커밋된_글자는_글자째()
    {
        var c = Typed("ㄱㅏ");
        c.PutText(' ');                      // "가 " — 조합 끝
        c.Backspace();                       // 사이띄기 지움
        Assert.Equal("가", c.Text);
        c.Backspace();                       // 커밋된 '가'는 글자째
        Assert.Equal("", c.Text);
    }

    [Fact]
    public void 도깨비불_뒤_백스페이스()
    {
        var c = Typed("ㄱㅏㅂㅅㅏ");         // 갑사
        c.Backspace();
        Assert.Equal("갑ㅅ", c.Text);
    }

    [Fact]
    public void 조합중_글과_전체_글이_따로_보인다()
    {
        var c = Typed("ㄴㅏㄴ");
        Assert.Equal("난", c.Composing);
        c.PutJamo('ㅏ');                     // 도깨비불 → "나" 커밋 + "나" 조합중
        Assert.Equal("나", c.Composing);
        Assert.Equal("나나", c.Text);
    }
}
