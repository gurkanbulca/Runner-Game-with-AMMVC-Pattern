using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Stackable : Collectable
{
    private Tween _tween; 
    private void Start()
    {
        Tween = ModelTransform.DOLocalMove(Vector3.up * 1.75f, 2).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        _tween = ModelTransform.DOLocalRotate(Vector3.up * 360, 2, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        _tween?.Kill();
    }

    public override void OnCollect()
    {
        base.OnCollect();
        ModelTransform.DOLocalMove(Vector3.zero, .5f);
        transform.DOScale(Vector3.one * .35f, 1);
    }
}