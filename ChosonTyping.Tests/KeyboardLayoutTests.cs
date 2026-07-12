using System.IO;
using ChosonTyping.Core;

namespace ChosonTyping.Tests;

public class KeyboardLayoutTests
{
    static string LayoutPath(string id) =>
        Path.Combine(AppContext.BaseDirectory, "data", "layouts", id + ".json");

    [Fact]
    public void 국규_R은_리을()
    {
        var l = KeyboardLayout.Load(LayoutPath("kukgyu"));
        Assert.Equal("ㄹ", l.JamoFor("R", shift: false));
    }

    [Fact]
    public void 국규_윗글쇠_Q는_쌍비읍()
    {
        var l = KeyboardLayout.Load(LayoutPath("kukgyu"));
        Assert.Equal("ㅃ", l.JamoFor("Q", shift: true));
    }

    [Fact]
    public void 국규_아래줄_C는_피읖_V는_치읓()
    {
        var l = KeyboardLayout.Load(LayoutPath("kukgyu"));
        Assert.Equal("ㅍ", l.JamoFor("C", shift: false));
        Assert.Equal("ㅊ", l.JamoFor("V", shift: false));
    }

    [Fact]
    public void 국규_대괄호글쇠는_겹화살괄호()
    {
        var l = KeyboardLayout.Load(LayoutPath("kukgyu"));
        Assert.Equal("《", l.JamoFor("[", shift: false));
        Assert.Equal("》", l.JamoFor("]", shift: false));
    }

    [Fact]
    public void 윗글쇠자모_없으면_기본자모로()
    {
        var l = KeyboardLayout.Load(LayoutPath("kukgyu"));
        Assert.Equal("ㅁ", l.JamoFor("W", shift: true));
    }

    [Fact]
    public void 창덕_쉼표는_통짜_와()
    {
        var l = KeyboardLayout.Load(LayoutPath("changdeok"));
        Assert.Equal("ㅘ", l.JamoFor(",", shift: false));
    }

    [Fact]
    public void 없는_글쇠는_널()
    {
        var l = KeyboardLayout.Load(LayoutPath("dubeol-std"));
        Assert.Null(l.JamoFor("1", shift: false));
    }

    [Fact]
    public void 세_배렬_모두_읽힌다()
    {
        foreach (var id in new[] { "kukgyu", "changdeok", "dubeol-std" })
        {
            var l = KeyboardLayout.Load(LayoutPath(id));
            Assert.NotEmpty(l.Keys);
            Assert.Equal(id, l.Id);
        }
    }
}
