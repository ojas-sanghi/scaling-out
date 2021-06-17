using System.ComponentModel;

namespace Enums
{
    public enum Stats
    {
        Hp, Delay, Def, Dodge, Dmg, Speed, Special
    }

    public enum Dinos
    {
        Mega, Tanky, Warrior, Gator
    }

    public enum Genes
    {
        None, Ice, Fire, Florida
    }

    public enum HomeScreenButtons
    {
        None, Map, Upgrades
    }

    public enum GameButtonModes
    {
        None, Quit, Play, RetryCombat, RetryStealth, StealthIce, StealthFire, ReturnHomeScreen, ReturnUpgrades, PlusDino, MinusDino, BuyDinos, ContinueConquest
    }

    public enum ShopUpgradeButtonModes
    {
        None, Hp, Delay, Def, Dodge, Dmg, Speed, Ice, Fire
    }

    public enum ArmyWeapons
    {
        Pistol, Rifle, Shotgun
    }

    public enum LaneTypes
    {
        RoadStraight, RoadEmpty
    }

    public enum USAStates
    {
        [Description("Alaska")] AK, 
        [Description("Alabama")] AL,
        [Description("Arkansas")] AR,
        [Description("Arizona")] AZ,
        [Description("California")] CA,
        [Description("Colorado")] CO,
        [Description("Connecticut")] CT,
        [Description("Deliware")] DE,
        [Description("Florida")] FL,
        [Description("Georgia")] GA,
        [Description("Hawaii")] HI,
        [Description("Iowa")] IA,
        [Description("Idaho")] ID,
        [Description("Illinois")] IL,
        [Description("Indiana")] IN,
        [Description("Kansas")] KS,
        [Description("Kentucky")] KY,
        [Description("Louisiana")] LA,
        [Description("Massachusetts")] MA,
        [Description("Maryland")] MD,
        [Description("Maine")] ME,
        [Description("Michigan")] MI,
        [Description("Minnessota")] MN,
        [Description("Missouri")] MO,
        [Description("Mississippi")] MS,
        [Description("Montana")] MT,
        [Description("North Carolina")] NC,
        [Description("North Dakota")] ND,
        [Description("Nebraska")] NE,
        [Description("New Hampshire")] NH,
        [Description("New Jersey")] NJ,
        [Description("New Mexico")] NM,
        [Description("Nevada")] NV,
        [Description("New York")] NY,
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
        [Description("Virginia")] VA,
        [Description("Vermont")] VT,
        [Description("Washington")] WA,
        [Description("Wisconson")] WI,
        [Description("West Virginia")] WV,
        [Description("Wyoming")] WY
    }

    // TODO: add major cities
    public enum USACities
    {
        Tucson, Phoenix
    }

    public static class EnumToString
    {
        
        public static string GetUSAStateName(USAStates state)
        {
            var type = state.GetType();
            var memberInfo = type.GetMember(state.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return state.ToString();
        }

    }


}