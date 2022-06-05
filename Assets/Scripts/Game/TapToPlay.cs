using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// tap to play button on the game win screen.
/// </summary>
[RequireComponent(typeof(Button))]
public class TapToPlay : ElementOf<GameManager>
{
    private Button _button;
    [SerializeField] private Image background;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        background.DOFade(1, 0);
        _button.onClick.AddListener((()=> Master.PlayGame()));
    }

}
