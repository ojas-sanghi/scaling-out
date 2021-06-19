using Godot;

public class UpgradeScreen : Control
{
    Label name;
    TextureRect image;

    ShaderMaterial BWShader;
    Theme specialUpgradeTheme;

    public override void _Ready()
    {
        name = (Label)FindNode("Name");
        image = (TextureRect)FindNode("Image");

        BWShader = GD.Load<ShaderMaterial>("res://assets/shaders/BlackWhiteShaderMaterial.tres");
        specialUpgradeTheme = GD.Load<Theme>("res://src/GUI/themes/SpecialBGTheme.tres");

        Events.dinoUnlocked += OnDinoUnlocked;

        SetInfo();
        SetDisabledUIStatus();
    }

    public override void _ExitTree()
    {
        Events.dinoUnlocked -= OnDinoUnlocked;
    }

    void SetInfo()
    {
        // TODO: figure out a better way of doing this
        switch (ShopInfo.shopDino)
        {
            case Enums.Dinos.Tanky:
                name.Text = "TANKY DINO";
                image.Texture = GD.Load<Texture>("res://assets/dinos/tanky_dino/Armored_Dino_ICON.png");
                break;

            case Enums.Dinos.Warrior:
                name.Text = "WARRIOR DINO";
                image.Texture = GD.Load<Texture>("res://assets/dinos/warrior_dino/Tribal_Dino_icon.png");
                break;

            case Enums.Dinos.Mega:
                name.Text = "MEGA DINO";
                image.Texture = GD.Load<Texture>("res://assets/dinos/mega_dino/mega_dino.png");
                break;

                // TODO: add gator dino
        }

        // hide special upgrade panel if the dino doesn't have any
        if (!DinoInfo.Instance.GetDinoInfo(ShopInfo.shopDino).HasSpecial())
        {
            GetNode<Control>("SpecialUpgrade").Hide();
        }
    }

    void SetDisabledUIStatus()
    {
        ShaderMaterial shaderToChangeTo;
        Theme themeToChangeTo;
        CursorShape cursorToChangeTo;
        MouseFilterEnum mouseFilterToChangeTo;

        ShopUnlockButton unlockButton = GetNode<ShopUnlockButton>("ShopUnlockButton");
        Label lockedStatusLabel = GetNode<Label>("LockedStatus");

        // if locked
        if (!PlayerStats.dinosUnlocked.Contains(ShopInfo.shopDino))
        {
            shaderToChangeTo = BWShader;
            themeToChangeTo = null;
            unlockButton.Show();
            lockedStatusLabel.Show();
            cursorToChangeTo = CursorShape.Forbidden;
            mouseFilterToChangeTo = MouseFilterEnum.Ignore;
        }
        // if unlocked
        else
        {
            shaderToChangeTo = null;
            themeToChangeTo = specialUpgradeTheme;
            unlockButton.Hide();
            lockedStatusLabel.Hide();
            cursorToChangeTo = CursorShape.Arrow;
            mouseFilterToChangeTo = MouseFilterEnum.Stop;
        }

        image.Material = shaderToChangeTo;

        foreach (ShopStatUpgradeButton b in GetNode("StatUpgradeButtons").GetChildren())
        {
            b.GetNode<Sprite>("Img").Material = shaderToChangeTo;
            ((Label)b.FindNode("StatNum")).Material = shaderToChangeTo;
            ((Label)b.FindNode("Stat")).Material = shaderToChangeTo;
            b.GetNode<RichTextLabel>("CostIndicator").Material = shaderToChangeTo;

            b.MouseDefaultCursorShape = cursorToChangeTo;
            b.MouseFilter = mouseFilterToChangeTo;
        }

        foreach (ShopUpgradeButton b in GetNode("UpgradeButtons").GetChildren())
        {
            b.GetNode<Sprite>("Img").Material = shaderToChangeTo;
            ((RichTextLabel)b.FindNode("CostIndicator")).Material = shaderToChangeTo;

            b.MouseDefaultCursorShape = cursorToChangeTo;
            b.MouseFilter = mouseFilterToChangeTo;
        }

        Control specialUpgrade = GetNode<Control>("SpecialUpgrade");
        Panel specialUpgradePanel = specialUpgrade.GetNode<Panel>("Panel");
        specialUpgrade.GetNode<TextureRect>("Sprite").Material = shaderToChangeTo;
        specialUpgradePanel.Theme = themeToChangeTo;
    }

    void OnDinoUnlocked()
    {
        SetDisabledUIStatus();
    }
}