using Godot;

[Tool]
public class DinoSelectTileButton : TextureButton
{
    [Export] public Color edgeColor;
    [Export] public Enums.Dinos dino = Enums.Dinos.None;

    Vector2 origScale;
    int origIndex;
    Rect2 origRect;

    bool hovered = false;
    float enlargeFactor = 1.5f; // how much the button enlarges by when hovered

    public override void _Ready()
    {
        origScale = this.RectScale;
        origIndex = this.GetIndex();

        // get this button's rect
        // used later for hover detection
        origRect = GetChild<Control>(0).GetRect();
        origRect.Size /= 1.35f; // make it a bit smaller so that multiple buttons cant be simultaneously hovered

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

        // check if the mouse is over the button; ie, if we're being hovered
        hovered = origRect.HasPoint(GetLocalMousePosition());

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
            icon.RectScale = new Vector2(1.75f, 1.75f);
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
