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
        // don't do anything if this dino doesn't have a speical upgrade
        var dinoUpgradeInfo = DinoInfo.Instance.upgradesInfo[ShopInfo.shopDino];
        if (!dinoUpgradeInfo.HasSpecial())
        {
            SetProcess(false);
            Hide();
            return;
        }
        SetProcess(true);

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
        var associatedAbility = DinoInfo.Instance.dinoTypesAndAbilities[ShopInfo.shopDino];

        name.Text = EnumUtils.GetSpecialAbilityName(associatedAbility);
        description.Text = EnumUtils.GetSpecialAbilityDescription(associatedAbility);
        sprite.Texture = DinoInfo.Instance.specialAbilityIcons[associatedAbility];

        video.Stream = DinoInfo.Instance.specialAbilityVidPreviews[associatedAbility];

        video.Play();
        //? Maybe find a new video format for the preview vids
    }

    public override void _Process(float delta)
    {
        if (previewButton.IsHovered())
        {
            if (!video.Visible) video.Show();
            if (!video.IsPlaying()) video.Play();
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