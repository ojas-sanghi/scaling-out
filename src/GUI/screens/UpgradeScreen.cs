using Godot;

public class UpgradeScreen : Control
{
    Label name;
    TextureRect image;

    public override void _Ready()
    {
        name = (Label)FindNode("Name");
        image = (TextureRect)FindNode("Image");

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

}