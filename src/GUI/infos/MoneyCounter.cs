using Godot;

public class MoneyCounter : Control
{
    int goldAmt = ShopInfo.gold;
    Label num;

    public override void _Ready()
    {
        num = (Label)FindNode("Num");

        Events.coinGrabbed += _on_Coin_grabbed;
        Events.dinoUpgraded += UpdateGoldAmountFromGlobal;
    }

    public override void _ExitTree()
    {
        Events.coinGrabbed -= _on_Coin_grabbed;
        Events.dinoUpgraded -= UpdateGoldAmountFromGlobal;
    }

    //? I don't think this connection works
    void _on_Coin_grabbed(int value)
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
        num.Text = ShopInfo.gold.ToString();
    }


}