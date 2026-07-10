using System.Windows.Input;

namespace ChosonTyping.Core;

/// <summary>물리 글쇠(WPF Key) → 배렬 토큰. 어느 글쇠를 눌렀는지 프로그람이 직접 안다(설계서 1항).</summary>
public static class KeyMapper
{
    public static string? ToToken(Key key)
    {
        if (key >= Key.A && key <= Key.Z) return key.ToString();
        if (key >= Key.D0 && key <= Key.D9) return ((char)('0' + (key - Key.D0))).ToString();
        return key switch
        {
            Key.OemOpenBrackets => "[",
            Key.OemCloseBrackets => "]",
            Key.Oem5 => "\\",
            Key.OemSemicolon => ";",
            Key.OemQuotes => "'",
            Key.OemComma => ",",
            Key.OemPeriod => ".",
            Key.OemQuestion => "/",
            Key.OemMinus => "-",
            Key.OemPlus => "=",
            Key.Oem3 => "`",
            Key.Space => " ",
            _ => null,
        };
    }
}
