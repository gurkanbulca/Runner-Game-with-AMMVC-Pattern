using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StackableBar : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    [SerializeField] private float fillDuration;

    /// <summary>
    /// updates fill amount for stackable cell.
    ///  under 35% bar color = red.
    ///  between 35% and 100% bar color = yellow.
    ///  equal to 100% bar color = green.
    ///  fill animation handles by dotween framework.
    ///  Also color transition handles by dotween and also has 0.5 fixed duration.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdateUI(float fillAmount)
    {
        Color color;
        if (fillAmount <= .35f)
            color = Color.red;
        else if (fillAmount < 1)
            color = Color.yellow;
        else
            color = Color.green;
        
        fillBar.DOFillAmount(fillAmount, fillDuration);
        fillBar.DOColor(color, 0.5f);
    }
}