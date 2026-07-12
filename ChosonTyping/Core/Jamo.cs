namespace ChosonTyping.Core;

/// <summary>한글 자모 표와 유니코드 조합식(0xAC00 공식). 설계서 5항.</summary>
public static class Jamo
{
    public const string Cho  = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
    public const string Jung = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
    public const string Jong = "\0ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";

    /// <summary>두 타로 조합하는 겹모음: (첫 타, 둘째 타) → 겹모음.</summary>
    public static readonly Dictionary<(char, char), char> VowelPairs = new()
    {
        [('ㅗ', 'ㅏ')] = 'ㅘ',
        [('ㅗ', 'ㅐ')] = 'ㅙ',
        [('ㅗ', 'ㅣ')] = 'ㅚ',
        [('ㅜ', 'ㅓ')] = 'ㅝ',
        [('ㅜ', 'ㅔ')] = 'ㅞ',
        [('ㅜ', 'ㅣ')] = 'ㅟ',
        [('ㅡ', 'ㅣ')] = 'ㅢ',
    };

    /// <summary>겹받침 11종: (첫 자모, 둘째 자모) → 겹받침.</summary>
    public static readonly Dictionary<(char, char), char> JongPairs = new()
    {
        [('ㄱ', 'ㅅ')] = 'ㄳ',
        [('ㄴ', 'ㅈ')] = 'ㄵ',
        [('ㄴ', 'ㅎ')] = 'ㄶ',
        [('ㄹ', 'ㄱ')] = 'ㄺ',
        [('ㄹ', 'ㅁ')] = 'ㄻ',
        [('ㄹ', 'ㅂ')] = 'ㄼ',
        [('ㄹ', 'ㅅ')] = 'ㄽ',
        [('ㄹ', 'ㅌ')] = 'ㄾ',
        [('ㄹ', 'ㅍ')] = 'ㄿ',
        [('ㄹ', 'ㅎ')] = 'ㅀ',
        [('ㅂ', 'ㅅ')] = 'ㅄ',
    };

    public static bool IsChoseong(char c) => Cho.Contains(c);
    public static bool IsVowel(char c) => Jung.Contains(c);
    public static bool CanBeJong(char c) => Jong.IndexOf(c) > 0;

    /// <summary>조합에 쓰이는 자모인가 — 배렬이 《 같은 기호를 내는 글쇠와 구별하는 데 쓴다.</summary>
    public static bool IsJamo(char c) => IsChoseong(c) || IsVowel(c) || CanBeJong(c);

    public static char Compose(int cho, int jung, int jong) =>
        (char)(0xAC00 + (cho * 21 + jung) * 28 + jong);
}
