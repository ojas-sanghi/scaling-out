using Godot;

public class MoneyCounter : Control
{
    int goldAmt = PlayerStats.gold;
    Label num;

    public override void _Ready()
    {
        num = (Label)FindNode("Num");

        Events.coinGrabbed += OnCoinGrabbed;
        Events.dinoUpgraded += UpdateGoldAmountFromGlobal;
    }

    public override void _ExitTree()
    {
        Events.coinGrabbed -= OnCoinGrabbed;
        Events.dinoUpgraded -= UpdateGoldAmountFromGlobal;
    }

    void OnCoinGrabbed(int value)
    {
        goldAmt += value;
        UpdateGoldAmount();
    }

    void UpdateGoldAmount()
    {
        num.Text = goldAmt.ToString();
    }

    void UpdateGoldAmountFromGlobal()
    {
        num.Text = PlayerStats.gold.ToString();
    }


}