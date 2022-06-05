using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private void SetActiveGameWinPanel()
    {
        gameWinPanel.SetActive(true);
    }


    private void HandleStateChange(IGameState gameState)
    {
        SetActiveScorePanel(gameState);
    }


    private void SetActiveScorePanel(IGameState gameState)
    {
        var playState = gameState as GamePlayState;
        scorePanel.SetActive(playState);
    }

    private IEnumerator SetActiveTapToContinueWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tapToContinue.gameObject.SetActive(true);
    }

    private void SetActiveGameWinPanel(IGameState gameState)
    {
        var playState = gameState as GameWinState;
        gameWinPanel.SetActive(playState);
        tapToContinue.gameObject.SetActive(false);
        StartCoroutine(SetActiveTapToContinueWithDelay(2));
    }
}