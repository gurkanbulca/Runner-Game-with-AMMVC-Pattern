using System.Collections.Generic;
using UnityEngine;

public class StackableView : ElementOf<ScoreController>
{
    [SerializeField] private StackableBar barPrefab;

    private List<StackableBar> _stackableBars;

    private void Awake()
    {
        _stackableBars = new List<StackableBar>();
        var cellCount = Master.StackableCellCount;
        for (int i = 0; i < cellCount; i++)
        {
            var bar = Instantiate(barPrefab, transform);
            _stackableBars.Add(bar);
        }
    }

    private void OnEnable()
    {
        Master.OnScoreModelChanged += UpdateStackableBars;
        UpdateStackableBars(Master.Model);
    }

    private void OnDisable()
    {
        Master.OnScoreModelChanged -= UpdateStackableBars;
    }

    /// <summary>
    /// Updates stackable cells by score model.
    /// </summary>
    /// <param name="scoreModel"></param>
    private void UpdateStackableBars(ScoreModel scoreModel)
    {
        var stackCount = scoreModel.stack;
        var stackLimit = Master.StackLimit;
        var cellCapacity = stackLimit / _stackableBars.Count;
        foreach (var bar in _stackableBars)
        {
            var cellAmount = stackCount > cellCapacity ? cellCapacity : stackCount;
            bar.UpdateUI((float) cellAmount / cellCapacity);
            stackCount -= cellAmount;
        }
    }
}