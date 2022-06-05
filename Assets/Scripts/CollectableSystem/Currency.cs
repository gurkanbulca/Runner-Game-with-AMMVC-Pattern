using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Currency : Collectable
{
    private Tween _tween;

    private void Start()
    {
        _tween = ModelTransform.DORotate(Vector3.up * 360, 2, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.InOutSine);
    }

    public override void OnCollect()
    {
        base.OnCollect();
        ModelTransform.DOScale(Vector3.zero, .1f).OnComplete(() =>
        {
            _tween?.Kill();
            Destroy(gameObject);
        });
    }
}