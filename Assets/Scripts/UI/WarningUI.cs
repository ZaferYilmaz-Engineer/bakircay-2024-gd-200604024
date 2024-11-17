using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WarningUI : MonoBehaviour
{
    public static WarningUI Instance { get; private set; }
    
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI warningTransform;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowWarning()
    {
        warningTransform.transform.localScale = Vector3.one;
        
        var newColor = warningTransform.color;
        newColor.a = 0;
        warningTransform.color = newColor;
        
        panel.SetActive(true);

        warningTransform.DOKill();

        Sequence sequence = DOTween.Sequence();
        Tween textFadeInTween = warningTransform.DOFade(1, 0.5f);
        Tween textScaleTween = warningTransform.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(3, LoopType.Yoyo);
        Tween textFadeOutTween = warningTransform.DOFade(0, 0.5f).SetDelay(1);

        sequence.Append(textFadeInTween);
        sequence.Join(textScaleTween);
        sequence.Join(textFadeOutTween);

        sequence.onComplete += () =>
        {
            panel.SetActive(false);
        };
    }
}
