using Godot;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Websocket.Client;

public static class WebUtils
{
    public static WebsocketClient Socket { get; set; }

    public static ConcurrentQueue<object> MessageQueue { get; set; } = new();

    public static Character[] PlayerCharacters { get; set; }

    public static Character[] OpponentCharacters { get; set; }

    public static int PlayerActiveCharacter { get; set; }

    public static int OpponentActiveCharacter { get; set; }

    private static void SetupCharacterList(JsonNode node)
    {
        var playerCharacters = (JsonArray)node["player_characters"];
        PlayerCharacters = playerCharacters.Select(elem =>
        {
            return new Character((string)elem);
        }).ToArray();

        var opponentCharacters = (JsonArray)node["opponent_characters"];
        OpponentCharacters = opponentCharacters.Select(elem =>
        {
            return new Character((string)elem);
        }).ToArray();

        PlayerActiveCharacter = 0;
        OpponentActiveCharacter = 0;
    }

    public static Task ConnectAsync(string url)
    {
        Socket = new WebsocketClient(new Uri(url));

        Socket.MessageReceived.Subscribe(msg =>
        {
            if (msg.MessageType != WebSocketMessageType.Text)
            {
                return;
            }

            GD.Print($"Received websocket message: {msg.Text}");
            var root = JsonNode.Parse(msg.Text);
            object translatedMessage;
            switch ((string)root["type"])
            {
                case "CharacterList":
                    SetupCharacterList(root["data"]);
                    translatedMessage = new SetupMessage();
                    break;
                default:
                    translatedMessage = new object();
                    break;
            }

            MessageQueue.Enqueue(translatedMessage);
            Task.Run(() => Socket.Send("Received"));
        });

        return Socket.Start().ContinueWith(task =>
        {
            Socket.Send("Connected");
        });
    }
}
