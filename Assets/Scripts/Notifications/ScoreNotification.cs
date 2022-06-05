public static class ScoreNotification
{
    public const string _StackCountChanged = "stack_count_changed";
    public const string _StackDamaged = "stack_damaged";
    public const string _AttackWithStack = "stack_attack";

    public static bool Contains(string notificationString)
    {
        return notificationString == _StackCountChanged
               || notificationString == _StackDamaged
               || notificationString == _AttackWithStack;
    }
}