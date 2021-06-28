using System.Collections.Generic;
using Godot;
using b = Enums.Biomes;
using d = Enums.Dinos;

public class CitiesInfo : Node
{
    public static CitiesInfo Instance;

    public List<CityInfoResource> citiesList = new List<CityInfoResource>();
    public CityInfoResource currentCity; // TODO: set this in the Map when we make that

    public Dictionary<Enums.Biomes, List<Enums.Dinos>> biomeFavoredDinos;
    public Dictionary<Enums.Biomes, List<Enums.Dinos>> biomeRestrictedDinos;

    public CitiesInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        biomeFavoredDinos = new Dictionary<Enums.Biomes, List<Enums.Dinos>>()
        {
            {b.Desert, new List<d>() {d.None}},
            {b.Forest, new List<d>() {d.None}},
            {b.Grassland, new List<d>() {d.None}},
            {b.Hills, new List<d>() {d.None}},
            {b.Oceanside, new List<d>() {d.Gator}},
            {b.River, new List<d>() {d.Gator}},
        };

        biomeRestrictedDinos = new Dictionary<Enums.Biomes, List<Enums.Dinos>>()
        {
            {b.Desert, new List<d>() {d.Gator}},
            {b.Forest, new List<d>() {d.None}},
            {b.Grassland, new List<d>() {d.None}},
            {b.Hills, new List<d>() {d.None}},
            {b.Oceanside, new List<d>() {d.None}},
            {b.River, new List<d>() {d.None}},
        };



        //////////////////////
        // load the CityInfoResource files
        //////////////////////
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