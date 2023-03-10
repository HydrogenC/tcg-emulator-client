using Godot;
using System;
using static GlobalData;

public partial class SpinningLoad : TextureRect
{

    const float TWICEPI = 2f * Mathf.Pi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ConnectAsync("ws://127.0.0.1:9001");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Rotation = (Rotation + (float)delta * Mathf.Pi) % TWICEPI;

        MessageBase obj;
        if (Socket.IsRunning && MessageQueue.TryDequeue(out obj))
        {
            if (obj is SetupClientMessage setupGame)
            {
                SetupCharacterList(setupGame);
                GetTree().ChangeSceneToFile("res://MainScene.tscn");
            }
        }
    }
}
