using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public static WarningUI Instance { get; private set; }
    
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Image backgroundImage;

    private float backgroundAlpha;

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
        backgroundAlpha = backgroundImage.color.a;
        panel.SetActive(false);
    }

    public void ShowWarning()
    {
        warningText.transform.localScale = Vector3.one;
        
        var newColor = warningText.color;
        newColor.a = 0;
        warningText.color = newColor;

        var newBackgroundAlphaColor = backgroundImage.color;
        newBackgroundAlphaColor.a = 0;
        backgroundImage.color = newBackgroundAlphaColor;
        
        panel.SetActive(true);
        TouchManager.Instance.isTouchEnabled = false;

        warningText.DOKill();
        backgroundImage.DOKill();
        
        Sequence sequence = DOTween.Sequence();
        Tween backgroundFadeInTween = backgroundImage.DOFade(backgroundAlpha, 0.5f);
        Tween textFadeInTween = warningText.DOFade(1, 0.5f);
        Tween textScaleTween = warningText.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(3, LoopType.Yoyo);
        Tween textFadeOutTween = warningText.DOFade(0, 0.5f).SetDelay(1);
        Tween backgroundFadeOutTween = backgroundImage.DOFade(0, 0.5f);

        sequence.Append(backgroundFadeInTween);
        sequence.Join(textFadeInTween);
        sequence.Join(textScaleTween);
        sequence.Join(textFadeOutTween);
        sequence.Join(backgroundFadeOutTween);

        sequence.onComplete += () =>
        {
            TouchManager.Instance.isTouchEnabled = true;
            panel.SetActive(false);
        };
    }
}
