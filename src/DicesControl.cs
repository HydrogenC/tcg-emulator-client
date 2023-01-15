using Godot;
using System;
using static GlobalData;

public partial class DicesControl : Control
{
    const int SPACING = 320;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        int x_offset = -SPACING * ((int)Math.Ceiling(Dices.Length / 2.0) - 1) / 2;
        int y_offset = -160;
        var dicePrototype = GD.Load<PackedScene>("res://prefabs/Dice.tscn");
        for (int i = 0; i < Dices.Length; i++)
        {
            var diceInstance = dicePrototype.Instantiate() as TextureButton;
            

            if (x_offset > 0)
            {
                diceInstance.OffsetLeft = x_offset;
            }
            else
            {
                diceInstance.OffsetRight = x_offset;
            }

            if (y_offset > 0)
            {
                diceInstance.OffsetTop = y_offset;
                x_offset += SPACING;
            }
            else
            {
                diceInstance.OffsetBottom = y_offset;
            }

            AddChild(diceInstance);
            y_offset = -y_offset;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }
}
