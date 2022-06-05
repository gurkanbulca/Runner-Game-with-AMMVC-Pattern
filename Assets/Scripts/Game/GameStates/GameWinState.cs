public class GameWinState : ElementOf<Application>, IGameState
{
    private GameManager _gameManager;

    public void Handle(GameManager gameManager)
    {
        if (!_gameManager)
            _gameManager = gameManager;

        Master.Notify(GameNotification._GameWin);
    }
}