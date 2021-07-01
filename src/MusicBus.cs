using Godot;
using System;

public class MusicBus : Node
{
    // Class that turns the music down if debug, and up if release

    [Export] int releaseVolumeDb = -5;
    [Export] int debugVolumeDb = -80;

    public override void _Ready()
    {
        var busIndex = AudioServer.GetBusIndex("Master");

        if (OS.IsDebugBuild())
        {
            AudioServer.SetBusVolumeDb(busIndex, debugVolumeDb);
        }
        else
        {
            AudioServer.SetBusVolumeDb(busIndex, releaseVolumeDb);
        }
    }
}