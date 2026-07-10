namespace ChosonTyping.Core;

/// <summary>
/// 측정규칙(설계서 11항): 총타수는 지웠다 다시 친 것 포함 전부,
/// 정확도는 고친 뒤 마지막 상태 기준(친 자리만 견줌), 백스페이스는 타수에 넣지 않는다.
/// </summary>
public static class TypingStats
{
    /// <summary>타속(타/분) = 누른 글쇠수 ÷ 걸린시간(분).</summary>
    public static double Cpm(int strokes, TimeSpan elapsed) =>
        elapsed.TotalMinutes <= 0 ? 0 : strokes / elapsed.TotalMinutes;

    /// <summary>정확도(%) — 친 자리만 본보기글과 견준다. 안 친 자리는 진행률의 몫.</summary>
    public static double Accuracy(string expected, string typed)
    {
        int compared = Math.Min(expected.Length, typed.Length);
        if (compared == 0) return 100;
        int correct = 0;
        for (int i = 0; i < compared; i++)
            if (expected[i] == typed[i]) correct++;
        return correct * 100.0 / compared;
    }

    /// <summary>진행률(%) = 친 글자수 ÷ 본보기글 길이.</summary>
    public static double Progress(string expected, string typed) =>
        expected.Length == 0 ? 100 : Math.Min(typed.Length, expected.Length) * 100.0 / expected.Length;
}
