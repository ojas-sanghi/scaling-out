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

        SetInfo();
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

    }

    void _on_UnlockButton_pressed()
    {
        var specialUpgrade = GetNode<Control>("SpecialUpgrade");
        Panel specialUpgradePanel = specialUpgrade.GetNode<Panel>("Panel");

        ShaderMaterial shaderToChangeTo = BWShader;
        Theme themeToChangeTo = specialUpgradeTheme;
        

        if (image.Material == BWShader)
        {
            shaderToChangeTo = null;
        } 
        if (specialUpgradePanel.Theme == themeToChangeTo)
        {
            themeToChangeTo = null;
        }

        image.Material = shaderToChangeTo;

        foreach (ShopStatUpgradeButton b in GetNode("StatUpgradeButtons").GetChildren())
        {
            b.GetNode<Sprite>("Img").Material = shaderToChangeTo;
            ((Label)b.FindNode("StatNum")).Material = shaderToChangeTo;
            ((Label)b.FindNode("Stat")).Material = shaderToChangeTo;
            b.GetNode<RichTextLabel>("CostIndicator").Material = shaderToChangeTo;
        }

        foreach (ShopUpgradeButton b in GetNode("UpgradeButtons").GetChildren())
        {
            b.GetNode<Sprite>("Img").Material = shaderToChangeTo;
            ((RichTextLabel)b.FindNode("CostIndicator")).Material = shaderToChangeTo;
        }

        specialUpgradePanel.Theme = themeToChangeTo;
        specialUpgrade.GetNode<TextureRect>("Sprite").Material = shaderToChangeTo;

    }

}