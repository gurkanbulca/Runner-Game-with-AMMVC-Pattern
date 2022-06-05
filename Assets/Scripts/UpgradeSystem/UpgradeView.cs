using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : ElementOf<UpgradeController>
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button purchaseButton;

    private void Start()
    {
        purchaseButton.onClick.AddListener(TryUpgrade);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void TryUpgrade()
    {
        var result = Master.TryUpgrade();
        if (result)
            UpdateUI();
    }

    private void UpdateUI()
    {
        purchaseButton.interactable = Master.currency >= Master.price;
        priceText.text = Master.price.ToString();
    }
}