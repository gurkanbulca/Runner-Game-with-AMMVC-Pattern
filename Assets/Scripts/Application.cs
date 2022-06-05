using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Application : MonoBehaviour
{
    public event Action<string, Object[]> OnInputNotificationSent;
    public event Action<string, Object[]> OnCharacterNotificationSent;
    public event Action<string, Object[]> OnCollectorNotificationSent;
    public event Action<string, Object[]> OnGameStateNotificationSent;
    public event Action<string, Object[]> OnScoreNotificationSent;
    public event Action<string, Object[]> OnOrbitNotificationSent;


    private static Application _instance;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Notify(string notificationString, params Object[] payload)
    {
        if (InputNotification.Contains(notificationString))
        {
            OnInputNotificationSent?.Invoke(notificationString, payload);
        }
        else if (CharacterNotification.Contains(notificationString))
        {
            OnCharacterNotificationSent?.Invoke(notificationString, payload);
        }
        else if (CollectorNotification.Contains(notificationString))
        {
            OnCollectorNotificationSent?.Invoke(notificationString, payload);
        }
        else if (GameNotification.Contains(notificationString))
        {
            OnGameStateNotificationSent?.Invoke(notificationString, payload);
        }
        else if (ScoreNotification.Contains(notificationString))
        {
            OnScoreNotificationSent?.Invoke(notificationString, payload);
        }
        else if (OrbitNotification.Contains(notificationString))
        {
            OnOrbitNotificationSent?.Invoke(notificationString, payload);
        }
    }
}