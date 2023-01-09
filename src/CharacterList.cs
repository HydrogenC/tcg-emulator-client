using Godot;
using System;
using System.Linq;
using static WebUtils;

public partial class CharacterList : Control
{
    [Export]
    // true for player, false for opponent
    bool IsPlayer
    {
        get; set;
    }

    const int SPACING = 420;
    const int PADDING_Y = 40;
    const int PADDING_Y_ACTIVE = 80;
    Character[] characters;
    int activeCharacter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        characters = IsPlayer ? PlayerCharacters : OpponentCharacters;
        activeCharacter = IsPlayer ? PlayerActiveCharacter : OpponentActiveCharacter;

        var cardPrototype = GD.Load<PackedScene>($"res://prefabs/{(IsPlayer ? "Player" : "Opponent")}CharacterCard.tscn");
        int x_offset = -SPACING * ((characters.Length - 1) / 2);
        for (int i = 0; i < characters.Length; i++)
        {
            var cardInstance = cardPrototype.Instantiate() as TextureButton;
            var characterTexture = GD.Load<Texture2D>($"res://images/{characters[i].Name}_Character_Card.webp");
            cardInstance.TextureNormal = characterTexture;
            cardInstance.TexturePressed = characterTexture;

            int y_offset = i == activeCharacter ? PADDING_Y_ACTIVE : PADDING_Y;
            if (IsPlayer)
            {
                cardInstance.OffsetBottom = -y_offset;
            }
            else
            {
                cardInstance.OffsetTop = y_offset;
            }

            cardInstance.OffsetLeft = x_offset;
            AddChild(cardInstance);
            x_offset += SPACING;

            GD.Print($"Instantiated a card named {characters[i].Name}");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }
}
