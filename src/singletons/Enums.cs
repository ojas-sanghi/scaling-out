using System.ComponentModel;

namespace Enums
{
    public enum Stats
    {
        Hp, Delay, Def, Dodge, Dmg, Speed, Special
    }

    public enum Dinos
    {
        None, Mega, Tanky, Warrior, Gator
    }

    public enum Genes
    {
        None, Cryo, Fire, Florida
    }

    public enum SpecialAbilities
    {
        None, IceProjectile, FireProjectile
    }

    public enum HomeScreenButtons
    {
        None, Map, Upgrades
    }

    public enum GameButtonModes
    {
        None, Quit, Play, RetryCombat, RetryStealth, StealthIce, StealthFire, ReturnHomeScreen, ReturnUpgradeSelect, PlusDino, MinusDino, BuyDinos, ContinueConquest
    }

    public enum ShopUpgradeButtonModes
    {
        None, Hp, Delay, Def, Dodge, Dmg, Speed, Special
    }

    public enum ArmyGunTypes
    {
        Pistol, Rifle, Shotgun
    }

    public enum LaneTypes
    {
        RoadStraight, RoadEmpty
    }

    public enum Biomes
    {
        Desert, Forest, Grassland, Hills, River, Oceanside
    }

    public enum USAStates
    {
        [Description("Alabama")] AL,
        [Description("Alaska")] AK,
        [Description("Arizona")] AZ,
        [Description("Arkansas")] AR,
        [Description("California")] CA,
        [Description("Colorado")] CO,
        [Description("Connecticut")] CT,
        [Description("Delaware")] DE,
        [Description("District of Columbia")] DC,
        [Description("Florida")] FL,
        [Description("Georgia")] GA,
        [Description("Hawaii")] HI,
        [Description("Idaho")] ID,
        [Description("Illinois")] IL,
        [Description("Indiana")] IN,
        [Description("Iowa")] IA,
        [Description("Kansas")] KS,
        [Description("Kentucky")] KY,
        [Description("Louisiana")] LA,
        [Description("Maine")] ME,
        [Description("Maryland")] MD,
        [Description("Massachusetts")] MA,
        [Description("Michigan")] MI,
        [Description("Minnessota")] MN,
        [Description("Mississippi")] MS,
        [Description("Missouri")] MO,
        [Description("Montana")] MT,
        [Description("Nebraska")] NE,
        [Description("Nevada")] NV,
        [Description("New Hampshire")] NH,
        [Description("New Jersey")] NJ,
        [Description("New Mexico")] NM,
        [Description("New York")] NY,
        [Description("North Carolina")] NC,
        [Description("North Dakota")] ND,
        [Description("Ohio")] OH,
        [Description("Oklahoma")] OK,
        [Description("Oregon")] OR,
        [Description("Pennsylvania")] PA,
        [Description("Rhode Island")] RI,
        [Description("South Carolina")] SC,
        [Description("South Dakota")] SD,
        [Description("Tennessee")] TN,
        [Description("Texas")] TX,
        [Description("Utah")] UT,
        [Description("Vermont")] VT,
        [Description("Virginia")] VA,
        [Description("Washington")] WA,
        [Description("West Virginia")] WV,
        [Description("Wisconson")] WI,
        [Description("Wyoming")] WY
    }

    public enum USACities
    {
        // AL
        Birmingham, Montgomery,

        // AK
        Anchorage, Juneau,

        // AZ
        Tucson, Phoenix,

        // AR
        [Description("Little Rock")] LittleRock,

        // CA
        [Description("Los Angeles")] LosAngeles,
        [Description("San Francisco")] SanFrancisco,
        Sacramento,

        // CO
        Denver,

        // CT
        Bridgeport, Hartford,

        // DE
        Dover,

        // DC
        [Description("Washington, D.C.")] WashingtonDC,

        // FL
        Jacksonville, Miami, Orlando, Tallahassee,

        // GA
        Atlanta,

        // HI
        Honolulu,

        // ID
        Boise,

        // IL
        Chicago, Springfield,

        // IN
        Indianapolis,

        // IA
        [Description("Des Moines")] DesMoines,

        // KS
        Wichita,
        [Description("Kansas City")] KansasCity,
        Topeka,

        // KY
        Louisville, Lexington, Frankfort,

        // LA
        [Description("New Orleans")] NewOrleans,
        [Description("Baton Rouge")] BatonRouge,

        // ME
        Portland,
        Augusta,

        // MD
        Baltimore, Annapolis,

        // MA
        Cambridge, Boston,

        // MI
        Detroit, Lansing,

        // MN
        [Description("Minneapolis - Saint Paul")] MinneapolisSaintPaul,

        // MS
        Jackson,

        // MO
        [Description("Kansas City")] KansasCity2,
        [Description("Saint Louis")] SaintLouis,
        [Description("Jefferson City")] JeffersonCity,

        // MT
        Billings,
        Helena,

        // NE
        Omaha,
        Lincoln,

        // NV
        [Description("Las Vegas")] LasVegas,
        [Description("Carson City")] CarsonCity,

        // NH
        Manchester, Nashua, Concord,

        // NJ
        Newark,
        Trenton,

        // NM
        Albuquerque,
        [Description("Las Cruces")] LasCruces,
        Roswell,
        [Description("Santa Fe")] SantaFe,

        // NY
        [Description("New York City")] NewYorkCity,
        Buffalo,
        Albany,

        // NC
        Charlotte, Raleigh,

        // ND
        Fargo, Bismarck,

        // OH
        Cleveland, Columbus,

        // OK
        Tulsa,
        [Description("Oklahoma City")] OklahomaCity,

        // OR
        [Description("Portland")] Portland2,
        Salem,

        // PA
        Philadelphia, Pittsburgh, Harrisburg,

        // RI
        Providence,

        // SC
        Charleston, Columbia,

        // SD
        [Description("Sioux Falls")] SiouxFalls,
        Pierre,

        // TN
        Memphis, Nashville,

        // TX
        Houston,
        [Description("San Antonio")] SanAntonio,
        Dallas,
        [Description("Fort Worth")] FortWorth,
        Austin,

        // UT
        [Description("Salt Lake City")] SaltLakeCity,

        // VT
        Burlington, Montpelier,

        // VA
        [Description("Virginia Beach")] VirginiaBeach,
        Richmond,

        // WA
        Seattle, Spokane, Olympia,

        // WV
        [Description("Charleston")] Charleston2,

        // WI
        Milwaukee, Madison,

        // WY
        Cheyenne
    }



}