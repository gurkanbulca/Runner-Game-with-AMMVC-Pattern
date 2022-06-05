using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class OrbitController : ElementOf<Application>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject orbiterPrefab;
    [SerializeField] private Orbit[] orbits;
    [SerializeField] private ParticleSystem obstacleParticle;

    private int _targetOrbitIndex;
    private int _orbiterCount;
    private Queue<Obstacle> _obstacleQueue;

    private void Start()
    {
        _obstacleQueue = new Queue<Obstacle>();
        Master.OnCollectorNotificationSent += HandleCollectorNotification;
        Master.OnScoreNotificationSent += HandleScoreNotification;
        Master.OnGameStateNotificationSent += HandleGameStateNotification;
    }
    
    private void OnDestroy()
    {
        Master.OnCollectorNotificationSent -= HandleCollectorNotification;
        Master.OnScoreNotificationSent -= HandleScoreNotification;
        Master.OnGameStateNotificationSent -= HandleGameStateNotification;
    }

    /// <summary>
    /// on current game state translates to play state, creates starting stack.
    /// </summary>
    /// <param name="stringNotification"></param>
    /// <param name="payload"></param>
    private void HandleGameStateNotification(string stringNotification, Object[] payload)
    {
        if (stringNotification == GameNotification._GamePlay)
        {
            CreateOrbiter(playerData.startingStackAmount);
        }
    }
    /// <summary>
    /// Listens for obstacle hit and prize claim notifications.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>

    private void HandleScoreNotification(string notificationString, Object[] payload)
    {
        if (notificationString == ScoreNotification._StackDamaged)
        {
            var orbitDamage = payload[0] as OrbitDamage;
            if (orbitDamage)
                StartCoroutine(HandleOrbitDamage(orbitDamage));
        }
        else if (notificationString == ScoreNotification._AttackWithStack)
        {
            var orbitAttack = payload[0] as OrbitAttack;
            if (orbitAttack)
                StartCoroutine(HandleOrbitAttack(orbitAttack));
        }
    }

    /// <summary>
    /// On gets hit from obstacle. orbiters will be dropped from the orbits.
    /// </summary>
    /// <param name="orbitAttack"></param>
    /// <returns></returns>
    private IEnumerator HandleOrbitAttack(OrbitAttack orbitAttack)
    {
        for (int i = 0; i < orbitAttack.targets.Count; i++)
        {
            var target = orbitAttack.targets[0];
            if (_orbiterCount < orbitAttack.targetHealth)
                break;

            yield return AttackToTarget(target, orbitAttack.targetHealth);
            _orbiterCount -= orbitAttack.targetHealth;
            Master.Notify(OrbitNotification._OrbitAttackToPrize, target);
        }

        RemoveFromOrbit(_orbiterCount);
        Master.Notify(OrbitNotification._PrizeAnimationCompleted);
    }

    /// <summary>
    /// Tries attack to the prize. For every completely full cell, one prize can claimable.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private IEnumerator AttackToTarget(Transform target, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _targetOrbitIndex = (_targetOrbitIndex - 1) % orbits.Length;
            if (_targetOrbitIndex < 0)
                _targetOrbitIndex += orbits.Length;
            if (i + 1 == amount)
            {
                yield return orbits[_targetOrbitIndex].AttackToTarget(target, 1);
                Instantiate(obstacleParticle, target);
            }
            else
            {
                StartCoroutine(orbits[_targetOrbitIndex].AttackToTarget(target, 1));
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    /// <summary>
    /// Dotween animations for obstacle and stackables will be dropped from the orbits.
    /// </summary>
    /// <param name="orbitDamage"></param>
    /// <returns></returns>
    private IEnumerator HandleOrbitDamage(OrbitDamage orbitDamage)
    {
        yield return new WaitWhile(() => _obstacleQueue.Count == 0);
        while (_obstacleQueue.Count > 0)
        {
            var obstacle = _obstacleQueue.Dequeue();
            var obstacleTransform = obstacle.transform;
            obstacleTransform.SetParent(transform);
            var obstacleLocalPosition = obstacleTransform.localPosition;
            var horizontalMultiplier = Random.Range(0, 2) == 0 ? 1 : -1f;
            var path = new[]
            {
                new Vector3(obstacleLocalPosition.x + Random.Range(1.5f, 2.5f) * horizontalMultiplier,
                    obstacleLocalPosition.y / 2,
                    obstacleLocalPosition.z / 2),
                Vector3.zero
            };
            obstacle.transform.DOLocalPath(path, .3f, PathType.CatmullRom, gizmoColor: Color.green).SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    Instantiate(obstacleParticle, transform);
                    path = new[]
                    {
                        new Vector3(5 * horizontalMultiplier, 0, -5),
                        new Vector3(10 * horizontalMultiplier, 0, -10)
                    };
                    RemoveFromOrbit(orbitDamage.damage);
                    obstacle.transform.DOLocalPath(path, .5f, PathType.CatmullRom, gizmoColor: Color.green)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => { Destroy(obstacle.gameObject); });
                });
        }
    }

    /// <summary>
    /// Instantiate orbiter from prefab by count (input).
    /// </summary>
    /// <param name="count"></param>
    private void CreateOrbiter(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var orbiter = Instantiate(orbiterPrefab, transform).transform;
            orbiter.GetComponent<Stackable>().OnCollect();
            orbiter.localScale = Vector3.zero;
            AddToOrbit(orbiter);
        }
    }

    /// <summary>
    /// Handles the player interactions with stackables and obstacles.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleCollectorNotification(string notificationString, Object[] payload)
    {
        if (notificationString == CollectorNotification._CollectStackable)
        {
            var stackable = payload[0] as Stackable;
            if (stackable)
                AddToOrbit(stackable.transform);
        }
        else if (notificationString == CollectorNotification._CollectObstacle)
        {
            _obstacleQueue.Enqueue(payload[0] as Obstacle);
        }
    }


    /// <summary>
    /// Iterates orbits and attack orbiter to the next orbit.
    /// </summary>
    /// <param name="orbiter"></param>
    private void AddToOrbit(Transform orbiter)
    {
        orbits[_targetOrbitIndex].AddToOrbit(orbiter);
        _targetOrbitIndex = (_targetOrbitIndex + 1) % orbits.Length;
        _orbiterCount++;
    }

    /// <summary>
    /// Remove orbiter from the orbiters by iteration (opposite order to adding process) and also count (input).
    /// </summary>
    /// <param name="count"></param>
    public void RemoveFromOrbit(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _targetOrbitIndex = (_targetOrbitIndex - 1) % orbits.Length;
            if (_targetOrbitIndex < 0)
                _targetOrbitIndex += orbits.Length;
            orbits[_targetOrbitIndex].RemoveFromOrbit(1);
        }

        _orbiterCount--;
    }
}