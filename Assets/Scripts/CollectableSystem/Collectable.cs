using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Collectable : MonoBehaviour
{
    private Collider _collider;
    private Transform _modelTransform;
    
    protected Transform ModelTransform
    {
        get
        {
            if (_modelTransform == null)
            {
                _modelTransform = transform.GetChild(0);
            }

            return _modelTransform;
        }
    }

    protected Tween Tween;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public virtual void OnCollect()
    {
        Tween?.Kill();
        _collider.enabled = false;
    }
}