using Godot;

public class SpecialUpgrade : Control
{
    Label name;
    Label description;
    TextureRect sprite;
    Button previewButton;
    VideoPlayer video;

    public override void _Ready()
    {
        name = (Label)FindNode("name");
        description = (Label)FindNode("Description");
        sprite = (TextureRect)FindNode("Sprite");
        previewButton = (Button)FindNode("PreviewButton");
        video = (VideoPlayer)FindNode("VideoPlayer");

        video.Hide();
        SetText();
    }


    void SetText()
    {
        switch (ShopInfo.shopDino)
        {
            case Enums.Dinos.Tanky:
                name.Text = "Ice Projectile";
                description.Text = "Unlock an ice projectile. When activated, will launch from a random tanky dinosaur on the map. On impact, slows down the rate of fire of the army. Can be used once per round.";
                sprite.Texture = GD.Load<Texture>("res://assets/dinos/misc/ice.png");
                video.Stream = GD.Load<VideoStream>("res://assets/abilities/previews/ice-preview.ogv");
                break;

            case Enums.Dinos.Warrior:
                name.Text = "Fire Projectile";
                description.Text = "Unlock a fire projectile. When activated, will launch from a random warrior dino on the map. On impact, stops the army from firing from a few seconds. Can be used once per round.";
                sprite.Texture = GD.Load<Texture>("res://assets/dinos/misc/fire.png");
                // video.Stream = GD.Load<VideoStream>("res://assets/abilities/previews/ice-preview.ogv");
                break;
        }
        video.Play();

        // TODO: add gator
        //? Maybe find a new video format for the preview vids
    }

    public override void _Process(float delta)
    {
        if (previewButton.IsHovered())
        {
            if (!video.Visible) video.Show();
            if (!video.IsPlaying()) video.Play() ;
        }
        else
        {
            if (video.Visible) video.Hide();
            if (video.IsPlaying()) video.Stop();
        }
    }

    void OnVideoPlayerFinished()
    {
        video.Play();
    }


}