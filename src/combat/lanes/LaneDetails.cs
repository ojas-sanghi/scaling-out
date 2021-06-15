using Godot;

/**
Class that holds a Lane's image and Curve info

Used to hold info about the lanes in LaneInfo.cs
**/
public class LaneDetails
{
    public Texture image;
    public Curve2D curve;

    public LaneDetails(Texture img, Curve2D crv)
    {
        image = img;
        curve = crv;
    }
}