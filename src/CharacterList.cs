using Godot;
using System;
using System.Linq;
using static GlobalData;

public partial class CharacterList : Control
{
    [Export]
    // true for player, false for opponent
    bool IsPlayer
    {
        get; set;
    }

    const int SPACING = 400;
    const int PADDING_Y = 40;
    const int PADDING_Y_ACTIVE = 100;
    Character[] characters;
    CharacterCard[] cards;
    int activeCharacter;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        characters = IsPlayer ? PlayerCharacters : OpponentCharacters;
        cards = new CharacterCard[characters.Length];
        activeCharacter = IsPlayer ? PlayerActiveCharacter : OpponentActiveCharacter;

        var cardPrototype = GD.Load<PackedScene>($"res://prefabs/{(IsPlayer ? "Player" : "Opponent")}CharacterCard.tscn");
        int x_offset = -SPACING * ((characters.Length - 1) / 2);
        for (int i = 0; i < characters.Length; i++)
        {
            var cardInstance = cardPrototype.Instantiate() as CharacterCard;
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
            cards[i] = cardInstance;

            x_offset += SPACING;
            GD.Print($"Instantiated a card named {characters[i].Name}");
        }
    }

    void CheckIfActiveChanged()
    {
        var newActive = IsPlayer ? PlayerActiveCharacter : OpponentActiveCharacter;

        if (activeCharacter != newActive)
        {
            var oldActiveCard = cards[activeCharacter];
            var newActiveCard = cards[newActive];

            if (IsPlayer)
            {
                oldActiveCard.OffsetBottom = -PADDING_Y;
                newActiveCard.OffsetBottom = -PADDING_Y_ACTIVE;
            }
            else
            {
                oldActiveCard.OffsetTop = PADDING_Y;
                newActiveCard.OffsetTop = PADDING_Y_ACTIVE;
            }

            activeCharacter = newActive;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CheckIfActiveChanged();
    }
}
