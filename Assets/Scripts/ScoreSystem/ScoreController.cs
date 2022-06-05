using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class ScoreController : ElementOf<Application>
{
    [SerializeField] private PlayerData playerData;
    [field: SerializeField] public int StackableCellCount { get; private set; }

    public ScoreModel Model { get; private set; }
    public int StackLimit => playerData.stackLimit;

    public event Action<ScoreModel> OnScoreModelChanged;

    private void Awake()
    {
        Model = ScriptableObject.CreateInstance<ScoreModel>();
        Model.currency = playerData.currencyAmount;
    }

    private void Start()
    {
        IncreaseStack(playerData.startingStackAmount);
    }

    private void OnEnable()
    {
        Master.OnGameStateNotificationSent += HandleGameStateNotification;
        Master.OnCollectorNotificationSent += HandleCollectorNotification;
    }


    private void OnDisable()
    {
        Master.OnGameStateNotificationSent -= HandleGameStateNotification;
        Master.OnCollectorNotificationSent -= HandleCollectorNotification;
    }

    /// <summary>
    /// Listens collect notifications and handle score values (stackable and currency).
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleCollectorNotification(string notificationString, Object[] payload)
    {
        if (notificationString == CollectorNotification._CollectStackable)
        {
            IncreaseStack(1);
        }
        else if (notificationString == CollectorNotification._CollectCurrency)
        {
            IncreaseCurrency(1);
        }
        else if (notificationString == CollectorNotification._CollectObstacle)
        {
            TryLoseStack();
        }
    }

    /// <summary>
    /// Increases currency amount by input. And also ivokes an ection for UIs.
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseCurrency(int amount)
    {
        playerData.currencyAmount += amount;
        Model.currency = playerData.currencyAmount;
        OnScoreModelChanged?.Invoke(Model);
    }

    /// <summary>
    /// Increases stack by input until to stack limit.
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseStack(int amount)
    {
        Model.stack = Mathf.Min(StackLimit, Model.stack + amount);
        OnScoreModelChanged?.Invoke(Model);
        Master.Notify(ScoreNotification._StackCountChanged, Model);
    }

    /// <summary>
    /// decrease loose stackable amount from stack.
    /// </summary>
    private void TryLoseStack()
    {
        var amount = GetLooseStackAmount();
        if (amount > 0)
        {
            Model.stack -= amount;
            OnScoreModelChanged?.Invoke(Model);
            Master.Notify(ScoreNotification._StackCountChanged, Model);
        }

        var orbitDamage = ScriptableObject.CreateInstance<OrbitDamage>();
        orbitDamage.damage = amount;
        Master.Notify(ScoreNotification._StackDamaged, orbitDamage);
    }

    /// <summary>
    /// loose stack means stackables on the last filled cell.
    /// </summary>
    /// <returns></returns>
    private int GetLooseStackAmount()
    {
        var stack = Model.stack;
        if (stack == 0)
        {
            return 0;
        }

        var cellSize = StackLimit / StackableCellCount;
        var looseAmount = stack % cellSize;

        return looseAmount == 0 ? cellSize : looseAmount;
    }

    /// <summary>
    /// on transition to play state, gets starting stack amount from player data and send action to UIs.
    /// And also send a notification with current score model.
    /// </summary>
    /// <param name="notificationString"></param>
    /// <param name="payload"></param>
    private void HandleGameStateNotification(string notificationString, Object[] payload)
    {
        if (notificationString == GameNotification._GamePlay)
        {
            Model.stack = playerData.startingStackAmount;
            OnScoreModelChanged?.Invoke(Model);
            Master.Notify(ScoreNotification._StackCountChanged, Model);
        }
        else if (notificationString == GameNotification._GameWin)
        {
            var orbitAttack = ScriptableObject.CreateInstance<OrbitAttack>();
            orbitAttack.targetHealth = StackLimit / StackableCellCount;
            var finishController = MonoBehaviour.FindObjectOfType<FinishController>();
            orbitAttack.targets = finishController.Prizes;
            Master.Notify(ScoreNotification._AttackWithStack, orbitAttack);
        }
        else if (notificationString == GameNotification._GameMenu)
        {
            Model.currency = playerData.currencyAmount;
        }
    }

    /// <summary>
    /// Checks current currency amount. If it is equal or greater than input, decreases it by amount.
    /// Then invoke an event for UIs.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool TrySpendCurrency(int amount)
    {
        if (playerData.currencyAmount < amount) return false;

        playerData.currencyAmount -= amount;
        Model.currency = playerData.currencyAmount;
        OnScoreModelChanged?.Invoke(Model);
        return true;
    }
}