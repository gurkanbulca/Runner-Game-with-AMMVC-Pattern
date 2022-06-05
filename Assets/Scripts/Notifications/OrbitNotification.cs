public static class OrbitNotification
{
    public const string _OrbitAttackToPrize = "orbit_attack";
    public const string _PrizeAnimationCompleted = "prize_animation_completed";

    public static bool Contains(string notificationString)
    {
        return notificationString == _OrbitAttackToPrize
               || notificationString == _PrizeAnimationCompleted;
    }
}