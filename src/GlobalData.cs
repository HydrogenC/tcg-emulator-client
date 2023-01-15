using Godot;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Websocket.Client;

public static class GlobalData
{
    public static readonly string[] ElementNames = new string[]
    {
        "any",
        "electro",
        "hydro",
        "pyro",
        "cryo",
        "anemo",
        "geo",
        "dendro",
        "void"
    };

    public static int[] Dices
    {
        get; set;
    } = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static WebsocketClient Socket { get; set; }

    public static ConcurrentQueue<MessageBase> MessageQueue { get; set; } = new();

    public static Character[] PlayerCharacters { get; set; }

    public static Character[] OpponentCharacters { get; set; }

    public static int PlayerActiveCharacter { get; set; }

    public static int OpponentActiveCharacter { get; set; }

    public static void SetupCharacterList(SetupClientMessage msg)
    {
        PlayerCharacters = msg.PlayerCharacters.Select(elem =>
        {
            return new Character(elem);
        }).ToArray();

        OpponentCharacters = msg.OpponentCharacters.Select(elem =>
        {
            return new Character(elem);
        }).ToArray();

        PlayerActiveCharacter = 0;
        OpponentActiveCharacter = 0;
    }

    public static Type GetDeserializeType(string type)
    {
        switch (type)
        {
            case "SetupClient":
                return typeof(SetupClientMessage);
            default:
                return typeof(MessageBase);
        }
    }

    public static Task ConnectAsync(string url)
    {
        Socket = new WebsocketClient(new Uri(url))
        {
            ErrorReconnectTimeout = TimeSpan.FromSeconds(20)
        };

        Socket.MessageReceived.Subscribe(msg =>
        {
            if (msg.MessageType != WebSocketMessageType.Text)
            {
                return;
            }

            GD.Print($"Received websocket message: {msg.Text}");
            var root = JsonNode.Parse(msg.Text);
            MessageBase translatedMessage = JsonSerializer.Deserialize(
                root["data"].ToString(),
                GetDeserializeType((string)root["type"]),
                SourceGenerationContext.Default
                ) as MessageBase;

            // Task.Run(() => Socket.Send("Received"));
            MessageQueue.Enqueue(translatedMessage);
        });

        return Socket.Start().ContinueWith(task =>
        {
            var jsonString = "{\"type\": \"JoinRoom\", \"room\": 100}";
            Socket.Send(jsonString);
        });
    }
}
