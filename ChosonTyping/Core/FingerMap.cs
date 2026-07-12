namespace ChosonTyping.Core;

/// <summary>
/// 글쇠 토큰 → 손가락 이름표(ID). 자리련습의 손가락 안내(설계서 6항)에 쓴다.
/// 이름은 Loc("finger.{id}")로 번역한다. ID: lp lr lm li ri rm rr rp th.
/// </summary>
public static class FingerMap
{
    static readonly Dictionary<string, string> Map = new();

    static FingerMap()
    {
        void Add(string id, params string[] toks)
        {
            foreach (var t in toks) Map[t] = id;
        }

        Add("lp", "`", "1", "Q", "A", "Z");        // 왼손 새끼
        Add("lr", "2", "W", "S", "X");             // 왼손 약
        Add("lm", "3", "E", "D", "C");             // 왼손 가운데
        Add("li", "4", "5", "R", "T", "F", "G", "V", "B"); // 왼손 집게
        Add("ri", "6", "7", "Y", "U", "H", "J", "N", "M"); // 오른손 집게
        Add("rm", "8", "I", "K", ",");             // 오른손 가운데
        Add("rr", "9", "O", "L", ".");             // 오른손 약
        Add("rp", "0", "-", "=", "P", ";", "'", "[", "]", "\\", "/"); // 오른손 새끼
        Map[" "] = "th";                           // 엄지
    }

    /// <summary>글쇠 토큰의 손가락 ID(없으면 빈 문자열).</summary>
    public static string For(string token) => Map.TryGetValue(token, out var f) ? f : "";
}
