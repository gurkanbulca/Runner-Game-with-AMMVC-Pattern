/// <summary>
/// Interface for game states.
/// </summary>
public interface IGameState
{
    void Handle(GameManager gameManager);
}