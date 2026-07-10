using System.Windows.Input;
using ChosonTyping.Core;

namespace ChosonTyping.Tests;

public class KeyMapperTests
{
    [Theory]
    [InlineData(Key.Q, "Q")]
    [InlineData(Key.M, "M")]
    [InlineData(Key.D9, "9")]
    [InlineData(Key.OemComma, ",")]
    [InlineData(Key.OemPeriod, ".")]
    [InlineData(Key.OemQuestion, "/")]
    [InlineData(Key.OemSemicolon, ";")]
    [InlineData(Key.OemQuotes, "'")]
    [InlineData(Key.OemOpenBrackets, "[")]
    [InlineData(Key.Oem5, "\\")]
    [InlineData(Key.Space, " ")]
    public void 글쇠_토큰(Key key, string token) =>
        Assert.Equal(token, KeyMapper.ToToken(key));

    [Fact]
    public void 기능글쇠는_널() => Assert.Null(KeyMapper.ToToken(Key.F5));
}
