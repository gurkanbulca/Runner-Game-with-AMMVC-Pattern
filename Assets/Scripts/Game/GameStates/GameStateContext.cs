using System;

/// <summary>
/// State machine for game states.
/// </summary>
public class GameStateContext
{
    public IGameState CurrentState { get; set; }

    private readonly GameManager _gameManager;

    public event Action<IGameState> OnCurrentStateChanged;

    public GameStateContext(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Transition(IGameState state)
    {
        CurrentState = state;
        CurrentState.Handle(_gameManager);
        OnCurrentStateChanged?.Invoke(CurrentState);
    }
}
