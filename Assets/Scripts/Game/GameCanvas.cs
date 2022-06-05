using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// level canvas controls play mode UI (scorePlay) and game win UI (gameWinPanel).
/// </summary>
public class GameCanvas : ElementOf<GameManager>
{
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private Button tapToContinue;

    private void Start()
    {
        Master.GameStateContext.OnCurrentStateChanged += HandleStateChange;
        Master.OnPrizeClaimCompleted += SetActiveGameWinPanel;
        SetActiveScorePanel(Master.GameStateContext.CurrentState);
        SetActiveGameWinPanel(Master.GameStateContext.CurrentState);
        tapToContinue.onClick.AddListener(() => Master.LevelUp());
    }

    private void OnDestroy()
    {
        Master.GameStateContext.OnCurrentStateChanged -= HandleStateChange;
        Master.OnPrizeClaimCompleted -= SetActiveGameWinPanel;
    }

    /// <summary>
    /// activates game win panel.
    /// </summary>
    private void SetActiveGameWinPanel()
    {
        gameWinPanel.SetActive(true);
    }


    /// <summary>
    /// handles game state changes.
    /// </summary>
    /// <param name="gameState"></param>
    private void HandleStateChange(IGameState gameState)
    {
        SetActiveScorePanel(gameState);
    }


    /// <summary>
    /// if game state equals to play state, activates score panel.
    /// </summary>
    /// <param name="gameState"></param>
    private void SetActiveScorePanel(IGameState gameState)
    {
        var playState = gameState as GamePlayState;
        scorePanel.SetActive(playState);
    }

    /// <summary>
    /// activate tap to continue button with delay input.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator SetActiveTapToContinueWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tapToContinue.gameObject.SetActive(true);
    }

    /// <summary>
    /// activates game win panel if current state equals to win state.
    /// </summary>
    /// <param name="gameState"></param>
    private void SetActiveGameWinPanel(IGameState gameState)
    {
        var playState = gameState as GameWinState;
        gameWinPanel.SetActive(playState);
        tapToContinue.gameObject.SetActive(false);
        StartCoroutine(SetActiveTapToContinueWithDelay(2));
    }
}