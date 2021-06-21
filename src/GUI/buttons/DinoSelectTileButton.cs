using Godot;
using System;

[Tool]
public class DinoSelectTileButton : TextureButton
{
    [Export] public Color edgeColor;
    [Export] public Enums.Dinos dino = Enums.Dinos.None;

    public override void _Ready()
    {
        setButtonInfo();
        this.FocusMode = FocusModeEnum.None;
    }

    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            if (edgeColor.a != 0)
            {
                GetNode<TextureRect>("Edge").Modulate = edgeColor;
            }
            return;
        }

        if (IsHovered())
        {
            this.RectScale = new Vector2((float)1.25, (float)1.25);
        }
        else
        {
            this.RectScale = Vector2.One;
        }
    }

    void setButtonInfo()
    {
        if (edgeColor.a != 0)
        {
            GetNode<TextureRect>("Edge").Modulate = edgeColor;
        }

        if (dino != Enums.Dinos.None)
        {
            var icon = GetNode<TextureRect>("Icon");
            var dinoIcon = DinoInfo.Instance.dinoIcons[dino];
            icon.Texture = dinoIcon;
            icon.RectScale = new Vector2((float)1.75, (float)1.75);
            icon.RectPosition = new Vector2(26, 39);
        }
    }

    void OnDinoSelectTileButtonPressed()
    {
        if (dino != Enums.Dinos.None)
        {
            ShopInfo.shopDino = dino;
            SceneChanger.Instance.GoToScene("res://src/GUI/screens/UpgradeScreen.tscn");
        }

    }

}
