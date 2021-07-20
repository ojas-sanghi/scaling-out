using Godot;
using Godot.Collections;

public class PlayerStatsResource : Resource
{
    [Export] public Array<Enums.Dinos> dinosUnlocked = new Array<Enums.Dinos>() { Enums.Dinos.Mega };
    [Export] public Array<Enums.Genes> genesFound = new Array<Enums.Genes>() { };

    public void SaveResource()
    {
        ResourceSaver.Save("user://PlayerStatsSave.tres", this);
    }

}