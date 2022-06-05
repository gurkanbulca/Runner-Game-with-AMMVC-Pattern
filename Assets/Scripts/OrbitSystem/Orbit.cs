using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float rotatePeriod;
    [SerializeField] private float repositionDuration = 1f;
    [SerializeField] private float orbitApsis;

    private List<Transform> orbiters = new List<Transform>();

    private void Start()
    {
        transform.DOLocalRotate(Vector3.up * 360, rotatePeriod, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }

    public void AddToOrbit(Transform orbiter)
    {
        orbiters.Add(orbiter);
        orbiter.SetParent(transform);
        RePositionOrbiters();
    }

    public void RemoveFromOrbit(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (orbiters.Count <= 0) return;
            var orbiter = orbiters[0];
            orbiters.RemoveAt(0);
            var randomX = Random.Range(-5f, 5f);
            var jumpPower = Random.Range(2f, 6f);
            orbiter.SetParent(null);
            orbiter.DOJump(new Vector3(randomX, -2, transform.position.z), jumpPower, 1, 1).OnComplete(() =>
            {
                Destroy(orbiter.gameObject);
                RePositionOrbiters();
            });
        }
    }

    public IEnumerator AttackToTarget(Transform target, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (orbiters.Count <= 0) yield break;
            {
                var orbiter = orbiters[0];
                orbiters.RemoveAt(0);
                orbiter.SetParent(null);
                var targetPosition = target.position;
                var orbiterPosition = orbiter.position;
                var randomX = Random.Range(-3f, 3f);
                var distanceY = targetPosition.y - orbiterPosition.y;
                var distanceZ = targetPosition.z - orbiterPosition.z;
                var path = new[]
                {
                    new Vector3(orbiterPosition.x + randomX, orbiterPosition.y + distanceY / 2,
                        orbiterPosition.z + distanceZ / 2),
                    targetPosition
                };
                yield return orbiter.DOPath(path, .7f, PathType.CatmullRom).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        Destroy(orbiter.gameObject);
                    }).WaitForCompletion();
            }
        }
    }

    private void RePositionOrbiters()
    {
        if (orbiters.Count == 0) return;
        var stepAngle = 360f / orbiters.Count;

        for (int i = 0; i < orbiters.Count; i++)
        {
            var orbiter = orbiters[i];
            var radian = Mathf.Deg2Rad * i * stepAngle;
            var targetPoint = new Vector3(Mathf.Sin(radian), 0,
                Mathf.Cos(radian));
            targetPoint = targetPoint * orbitApsis;
            orbiter.DOLocalMove(targetPoint, repositionDuration);
        }
    }
}