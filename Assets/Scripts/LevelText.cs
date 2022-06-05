using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LevelText : MonoBehaviour
{
    private TMP_Text _text;
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _text.text = "LEVEL " + playerData.currentLevelNumber;
    }
}
