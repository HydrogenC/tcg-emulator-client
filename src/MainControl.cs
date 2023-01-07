using Godot;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

public partial class MainControl : Control
{
    
    double totalDelta = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        totalDelta += delta;

        if (totalDelta > 2)
        {
            totalDelta -= 2;
        }
    }
}
