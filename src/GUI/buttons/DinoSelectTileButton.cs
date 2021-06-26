using Godot;
using System;

[Tool]
public class DinoSelectTileButton : TextureButton
{
    [Export] public Color edgeColor;
    [Export] public Enums.Dinos dino = Enums.Dinos.None;

    Vector2 origScale;
    int origIndex;

    bool hovered = false;
    float enlargeFactor = 1.5f; // how much the button enlarges by when hovered

    public override void _Ready()
    {
        origScale = this.RectScale;
        origIndex = this.GetIndex();

        setButtonInfo();
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

        // get this button's rect and check if we're still being hovered
        Rect2 buttonRect = GetChild<Control>(0).GetRect();
        hovered = buttonRect.HasPoint(GetLocalMousePosition());

        if (hovered)
        {
            this.RectScale = origScale * enlargeFactor;

            var parent = GetParent();
            var bigParent = GetParent().GetParent();

            // move this node and parent node to bottom of the respective trees they are in
            // this ensures they are drawn on top of everything else
            // and the buttonRect thing we do ensures the hover detection is still accurate
            parent.MoveChild(this, parent.GetChildCount() - 1);
            bigParent.MoveChild(parent, bigParent.GetChildCount() - 1);

            // TODO: fix multiple stuff being hovered at once
        }
        else
        {
            this.RectScale = origScale;
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
