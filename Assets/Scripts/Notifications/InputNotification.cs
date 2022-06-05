public static class InputNotification
{
    public const string _MouseMove = "mouse_move";

    public static bool Contains(string notificationString)
    {
        return notificationString == _MouseMove;
    }
}