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
        name.Text = EnumUtils.GetDinoName(ShopInfo.shopDino).ToUpper();
        image.Texture = DinoInfo.Instance.dinoIcons[ShopInfo.shopDino];
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
        if (!PlayerStats.Instance.dinosUnlocked.Contains(ShopInfo.shopDino))
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