using UnityEngine;

public class UpgradeController : ElementOf<ScoreController>
{
    [SerializeField] private PlayerData playerData;
    public int price => playerData.upgradeCost;
    public int currency => playerData.currencyAmount;

    /// <summary>
    /// upgrades starting stack if currency is enough for upgrade cost.
    /// </summary>
    /// <returns></returns>
    public bool TryUpgrade()
    {
        if (!Master.TrySpendCurrency(playerData.upgradeCost)) return false;
        playerData.startingStackAmount++;
        playerData.upgradeCost *= 2;
        return true;
    }
}