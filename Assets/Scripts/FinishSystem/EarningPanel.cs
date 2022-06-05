using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EarningPanel : ElementOf<FinishController>
{
    [SerializeField] private TMP_Text normalPrizeText, bigPrizeText, totalPrizeText;
    [SerializeField] private float animationDuration;

    private FinishController.EarnedPrize _earnedPrize;

    private void Start()
    {
        _earnedPrize = Master.earnedPrize;
        normalPrizeText.text = "+0";
        bigPrizeText.text = "+0";
        totalPrizeText.text = "+0";
        StartCoroutine(AnimateUI());
    }

    private IEnumerator AnimateUI()
    {
        yield return AnimateText(normalPrizeText, _earnedPrize.NormalPrize);
        yield return AnimateText(bigPrizeText, _earnedPrize.BigPrize);
        yield return AnimateText(totalPrizeText, _earnedPrize.NormalPrize + _earnedPrize.BigPrize);
    }

    private IEnumerator AnimateText(TMP_Text prizeText, int prizeAmount)
    {
        if (prizeAmount == 0)
        {
            yield return new WaitForSeconds(.25f);
            yield break;
        }

        var stepDuration = animationDuration / prizeAmount;
        for (int i = 0; i < prizeAmount; i++)
        {
            yield return new WaitForSeconds(stepDuration);
            prizeText.text = "+" + (i + 1);
        }
    }
}