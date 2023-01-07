using Godot;
using System;
using System.Net.WebSockets;

public partial class SpinningLoad : TextureRect
{
    const float TWICEPI = 2f * Mathf.Pi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        WebUtils.ConnectAsync("ws://127.0.0.1:9001");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Rotation = (Rotation + (float)delta * Mathf.Pi) % TWICEPI;

        object obj;
        if (WebUtils.Socket.IsRunning && WebUtils.MessageQueue.TryDequeue(out obj))
        {
            if (obj is SetupMessage)
            {
                GetTree().ChangeSceneToFile("res://MainScene.tscn");
            }
        }
    }
}