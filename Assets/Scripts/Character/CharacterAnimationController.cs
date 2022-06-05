using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : ElementOf<Application>
{
    [SerializeField] private PlayerData playerData;

    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Weight = Animator.StringToHash("Weight");
    private static readonly int Dance = Animator.StringToHash("Dance");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Master.OnCharacterNotificationSent += HandleCharacterNotification;
        Master.OnScoreNotificationSent += HandleScoreNotification;
        Master.OnGameStateNotificationSent += HandleGameStateNotification;
    }


    private void OnDisable()
    {
        Master.OnCharacterNotificationSent -= HandleCharacterNotification;
        Master.OnScoreNotificationSent -= HandleScoreNotification;
        Master.OnGameStateNotificationSent -= HandleGameStateNotification;
    }


    /// <summary>
    /// play dance animation on game state translate to win.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleGameStateNotification(string notificationString, Object[] payload)
    {
        if (notificationString == GameNotification._GameWin)
        {
            _animator.SetTrigger(Dance);
        }
    }


    /// <summary>
    /// adjust weight value by carrying stackables.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleScoreNotification(string notificationString, Object[] payload)
    {
        if (notificationString != ScoreNotification._StackCountChanged) return;

        var scoreModel = payload[0] as ScoreModel;
        if (!scoreModel) return;
        var stackPercent = scoreModel.stack / (float) playerData.stackLimit;
        _animator.SetFloat(Weight, stackPercent);
    }

    /// <summary>
    /// adjust speed value by character movement speed.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleCharacterNotification(string notificationString, Object[] payload)
    {
        if (notificationString != CharacterNotification._VerticalMovement) return;
        var acceleration = ((CharacterMovementModel) payload[0]).acceleration;
        _animator.SetFloat(Speed, acceleration);
    }
}