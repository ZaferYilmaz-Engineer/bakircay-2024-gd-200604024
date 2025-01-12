using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillButtonUI firstSkillButton;
    [SerializeField] private SkillButtonUI secondSkillButton;
    [SerializeField] private Image skillDurationImage;
    [SerializeField] private GameObject skillDurationObject;

    private void Start()
    {
        BaseSkill.OnAnySkillActivated += OnAnySkillActivated;
        skillDurationObject.SetActive(false);
    }

    private void OnAnySkillActivated(float duration)
    {
        firstSkillButton.SetInteractable(false);
        secondSkillButton.SetInteractable(false);
        
        skillDurationObject.SetActive(true);
        skillDurationImage.fillAmount = 1f;
        skillDurationImage.DOFillAmount(0f, duration).SetEase(Ease.Linear).onComplete += () =>
        {
            skillDurationObject.SetActive(false);
            
            firstSkillButton.SetInteractable(true);
            secondSkillButton.SetInteractable(true);
        };
    }

    private void OnDestroy()
    {
        BaseSkill.OnAnySkillActivated -= OnAnySkillActivated;
    }
}
