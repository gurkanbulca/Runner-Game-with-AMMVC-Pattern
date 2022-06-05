public class GameWinState : ElementOf<Application>, IGameState
{
    private GameManager _gameManager;

    /// <summary>
    /// triggers on current state changed to GameWinState.
    /// Send an notification to Application.
    /// </summary>
    /// <param name="gameManager"></param>
    public void Handle(GameManager gameManager)
    {
        if (!_gameManager)
            _gameManager = gameManager;

        Master.Notify(GameNotification._GameWin);
    }
}