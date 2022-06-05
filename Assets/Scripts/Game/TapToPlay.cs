using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
