public static class GameNotification
{
    public const string _GameMenu = "game_menu";
    public const string _GamePlay = "game_play";
    public const string _GamePause = "game_pause";
    public const string _GameWin = "game_win";
    public const string _GameLose = "game_lose";


    public static bool Contains(string notificationString)
    {
        return notificationString == _GamePlay
               || notificationString == _GameMenu
               || notificationString == _GamePause
               || notificationString == _GameWin
               || notificationString == _GameLose;
    }
}