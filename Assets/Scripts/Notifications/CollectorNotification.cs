public static class CollectorNotification
{
    public const string _CollectStackable = "collect_stackable";
    public const string _CollectCurrency = "collect_currency";
    public const string _CollectObstacle = "collect_obstacle";

    public static bool Contains(string notificationString)
    {
        return notificationString == _CollectStackable
               || notificationString == _CollectCurrency
               || notificationString == _CollectObstacle;
    }
}
