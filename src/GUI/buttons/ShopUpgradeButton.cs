using System.Collections.Generic;
using System.Linq;
using Godot;

using b = Enums.ShopUpgradeButtonModes;
using s = Enums.Stats;

[Tool]
public class ShopUpgradeButton : Button
{
    // One button mode is the exported one; it has "None" and separate options for special abilities
    // the other is the button mode that goes off the Enums.stats
    // we need to convert it since Enums.stats is used in all the DinoInfo.cs files and everything
    [Export] public b exportedButtonMode;
    public Enums.Stats statButtonMode;

    int maxSquares;
    bool firstRun = true;

    // this is checked by CostIndicator
    // used to wait for the info to be set and for the exportedButtonMode to be "special" if needed
    // once this is true, the cost indicator retrieves the info needed
    public bool infoSet = false;

    protected Color hpRed = new Color("e61a1a");
    protected Color delayBlue = new Color("1a82e6");

    protected int goldCost;
    protected int geneCost;

    float containerLength;
    float containerHeight;
    UpgradeInfo dinoInfo;

    Label name;
    protected Label stat;
    protected Label statNum;
    protected Sprite image;
    HBoxContainer squaresContainer;
    Tween tween;

    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            return;
        }

        if (exportedButtonMode == b.None)
        {
            GD.PushError("You must set exportedButtonMode for ShopUpgradeButton!");
            GD.PrintStack();
            GetTree().Quit(1);
        }

        name = (Label)FindNode("Name");
        stat = (Label)FindNode("Stat");
        statNum = (Label)FindNode("StatNum");
        image = (Sprite)FindNode("Img");
        squaresContainer = (HBoxContainer)FindNode("Squares");
        tween = (Tween)FindNode("Tween");

        containerLength = squaresContainer.RectSize.x;
        containerHeight = squaresContainer.RectSize.y;

        dinoInfo = DinoInfo.Instance.GetDinoInfo(ShopInfo.shopDino);

        DoEverything();
    }


    public virtual void SetButtonInfo()
    {
        switch (exportedButtonMode)
        {
            case b.Dmg:
                name.Text = "Damage";
                stat.Text = "DPS";
                image.Texture = GD.Load<Texture>("res://assets/abilities/fire.png");
                statButtonMode = Enums.Stats.Dmg;
                break;
            case b.Def:
                name.Text = "Defense";
                stat.Text = "x";
                image.Texture = GD.Load<Texture>("res://assets/abilities/health.png");
                statButtonMode = Enums.Stats.Def;
                break;
            case b.Speed:
                name.Text = "Speed";
                stat.Text = "KM/h";
                image.Texture = GD.Load<Texture>("res://assets/abilities/speed.png");
                statButtonMode = Enums.Stats.Speed;
                break;
            case b.Special:
                statButtonMode = Enums.Stats.Special;
                
                var dinoUpgradeInfo = DinoInfo.Instance.upgradesInfo[ShopInfo.shopDino];
                if (!dinoUpgradeInfo.HasSpecial()) {
                    Hide();
                    break;
                }


                name.Text = "Special";
                stat.Text = "";
                statNum.Text = "";
                
                // get the current special ability type using the info about the current dino screen we're on (shopinfo.shopdino)
                // then get the icon using that special ability type
                var ability = DinoInfo.Instance.dinoTypesAndAbilities[ShopInfo.shopDino];
                image.Texture = DinoInfo.Instance.specialAbilityIcons[ability];

                break;
        }
        infoSet = true;

        if (statButtonMode == Enums.Stats.Special)
        {
            statNum.Text = dinoInfo.GetSpecial();
        }
        else
        {
            statNum.Text = dinoInfo.GetStat(statButtonMode).ToString();
        }


        List<int> cost = dinoInfo.GetNextUpgradeCost(statButtonMode);
        goldCost = cost[0];
        geneCost = cost[1];
    }

    void ColorSquares(Color color)
    {
        var squaresList = squaresContainer.GetChildren().Cast<ColorRect>().ToList<ColorRect>();
        var filledSquares = dinoInfo.GetLevel(statButtonMode);

        if (statButtonMode == s.Hp)
        {
            color = hpRed;
        }
        else if (statButtonMode == s.Delay)
        {
            color = delayBlue;
        }

        for (int i = 0; i < filledSquares; i++)
        {
            if (filledSquares == maxSquares)
            {
                squaresList[i].Color = new Color(0, 1, 0, 1);
            }
            else
            {
                squaresList[i].Color = color;
            }
        }
    }

    void ColorSquares()
    {
        ColorSquares(new Color(1, 1, 1, 1));
    }

    void SetUpgradeSquares()
    {
        maxSquares = dinoInfo.GetMaxLevel(statButtonMode);

        for (int i = 0; i < maxSquares; i++)
        {
            var newSquare = new ColorRect();
            newSquare.Color = new Color(0, 0, 0, (float)0.5);
            newSquare.RectMinSize = new Vector2(containerLength / maxSquares, containerHeight);
            newSquare.RectSize = newSquare.RectMinSize;

            // don't make new squares every time
            if (firstRun)
            {
                squaresContainer.AddChild(newSquare);
            }
        }
        firstRun = false;
        ColorSquares();
    }

    void DoEverything()
    {
        SetButtonInfo();
        SetUpgradeSquares();
    }

    //////////////

    void StopUpgrading()
    {
        tween.SetActive(false);
        tween.ResetAll();
    }

    void OnUpgradeButtonButtonDown()
    {
        // don't do anything if max upgrades reached
        if (dinoInfo.IsMaxedOut(statButtonMode))
        {
            return;
        }
        // don't do anything if not enough gold/genes
        if (PlayerStats.gold < goldCost)
        {
            return;
        }
        if (PlayerStats.genes < geneCost)
        {
            return;
        }

        tween.InterpolateProperty(GetNode<TextureProgress>("TextureProgress"), "value", 0, 100, (float)1.5);
        tween.Start();
    }

    void OnUpgradeButtonButtonUp()
    {
        StopUpgrading();
    }

    void OnTweenTweenCompleted(object @object, NodePath key)
    {
        PlayerStats.gold -= goldCost;
        PlayerStats.genes -= geneCost;

        dinoInfo.Upgrade(statButtonMode);
        Events.publishDinoUpgraded();

        StopUpgrading();
        DoEverything();
    }

}