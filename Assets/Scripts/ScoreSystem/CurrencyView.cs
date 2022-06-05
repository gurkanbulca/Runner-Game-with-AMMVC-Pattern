using TMPro;

public class CurrencyView : ElementOf<ScoreController>
{
    private TMP_Text _text;


    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        Master.OnScoreModelChanged += HandleScoreModelChanged;

        HandleScoreModelChanged(Master.Model);
    }


    private void OnDisable()
    {
        Master.OnScoreModelChanged -= HandleScoreModelChanged;
    }

    private void HandleScoreModelChanged(ScoreModel scoreModel)
    {
        _text.text = scoreModel.currency.ToString();
    }
}