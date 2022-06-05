using UnityEngine;

public class UpgradeController : ElementOf<ScoreController>
{
    [SerializeField] private PlayerData playerData;
    public int price => playerData.upgradeCost;
    public int currency => playerData.currencyAmount;

    public bool TryUpgrade()
    {
        if (!Master.TrySpendCurrency(playerData.upgradeCost)) return false;
        playerData.startingStackAmount++;
        playerData.upgradeCost *= 2;
        return true;
    }
}