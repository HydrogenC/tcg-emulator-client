using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(SetupClientMessage))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

public class MessageBase { }

public class SetupClientMessage : MessageBase
{
    [JsonPropertyName("player_index")]
    public ulong PlayerIndex { get; set; }

    [JsonPropertyName("player_characters")]
    public string[] PlayerCharacters { get; set; }

    [JsonPropertyName("opponent_characters")]
    public string[] OpponentCharacters { get; set; }
}
