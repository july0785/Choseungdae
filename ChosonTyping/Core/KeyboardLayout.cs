using System.IO;
using System.Text.Json;

namespace ChosonTyping.Core;

public sealed record KeyDef(string? Base, string? Shift);

/// <summary>건반배렬 — data\layouts\*.json에서 읽는다. 배렬은 코드에 박지 않는다(설계서 2항).</summary>
public sealed class KeyboardLayout
{
    public string Name { get; init; } = "";
    public string Id { get; init; } = "";
    public string Type { get; init; } = "dubeol";
    public Dictionary<string, KeyDef> Keys { get; init; } = new();

    static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };

    public static KeyboardLayout Load(string path) =>
        JsonSerializer.Deserialize<KeyboardLayout>(File.ReadAllText(path), Opts)
        ?? throw new InvalidDataException($"배렬 화일을 읽을수 없습니다: {path}");

    /// <summary>글쇠 토큰과 윗글쇠 여부로 자모를 얻는다. 윗글쇠 자모가 없으면 기본 자모.</summary>
    public string? JamoFor(string keyToken, bool shift) =>
        Keys.TryGetValue(keyToken, out var k) ? (shift ? k.Shift ?? k.Base : k.Base) : null;
}
