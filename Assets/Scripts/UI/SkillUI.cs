using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button firstSkillButton;
    [SerializeField] private Button secondSkillButton;
    [SerializeField] private Image skillDurationImage;
    [SerializeField] private GameObject skillDurationObject;

    private void Start()
    {
        firstSkillButton.onClick.AddListener(() =>
        {
            SkillManager.Instance.TryActiveSkill(0);
        });
        
        secondSkillButton.onClick.AddListener(() =>
        {
            SkillManager.Instance.TryActiveSkill(1);
        });
        
        BaseSkill.OnAnySkillActivated += OnAnySkillActivated;
        skillDurationObject.SetActive(false);
    }

    private void OnAnySkillActivated(float duration)
    {
        firstSkillButton.interactable = false;
        secondSkillButton.interactable = false;
        
        skillDurationObject.SetActive(true);
        skillDurationImage.fillAmount = 1f;
        skillDurationImage.DOFillAmount(0f, duration).SetEase(Ease.Linear).onComplete += () =>
        {
            skillDurationObject.SetActive(false);
            
            firstSkillButton.interactable = true;
            secondSkillButton.interactable = true;
        };
    }

    private void OnDestroy()
    {
        BaseSkill.OnAnySkillActivated -= OnAnySkillActivated;
    }
}
