public static class CharacterNotification
{
    public const string _VerticalMovement = "vertical_movement";

    public static bool Contains(string notificationString)
    {
        return notificationString == _VerticalMovement;
    }
}