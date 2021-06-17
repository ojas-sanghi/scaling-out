using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class CitiesInfo : Node
{
    public static CitiesInfo Instance;

    public List<CityInfoResource> citiesList = new List<CityInfoResource>();
    // TODO: set this in the Map when we make that
    public CityInfoResource currentCity;

    public CitiesInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        string infoDirectory = "res://src/combat/levelsInfo/";

        // go through the levelsInfo directory and add each filename to a list
        // that list is then used to construct filepaths to load all the resources
        var filesList = new List<string>();
        Directory dir = new Directory();
        dir.Open(infoDirectory);
        dir.ListDirBegin(true);
        while (true)
        {
            var file = dir.GetNext();
            if (file == "")
            {
                dir.ListDirEnd();
                break;
            }
            else
            {
                filesList.Add(file);
            }
        }

        foreach (string s in filesList)
        {
            citiesList.Add(GD.Load<CityInfoResource>(infoDirectory + s));
        }
        
        // TODO: remove this when we have a Map system done to select cities and then attack those
        currentCity = citiesList[0];
    }
}