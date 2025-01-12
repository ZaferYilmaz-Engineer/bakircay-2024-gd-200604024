using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;
    [SerializeField] private GameObject cooldownGroup;
    [SerializeField] private Image lockedBackgroundImage;
    [SerializeField] private TMP_Text cooldownText;
    
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        BaseSkill.OnAnySkillInCooldown += OnSkillInCooldown;
        cooldownGroup.SetActive(false);
    }

    private void OnSkillInCooldown(SkillDataSO _skillDataSO)
    {
        if (skillDataSO != _skillDataSO)
        {
            return;
        }
        
        cooldownGroup.SetActive(true);
        lockedBackgroundImage.fillAmount = 1f;
        cooldownText.text = skillDataSO.cooldown.ToString();

        lockedBackgroundImage.DOFillAmount(0f, skillDataSO.cooldown).SetEase(Ease.Linear).onComplete += () =>
        {
            cooldownGroup.SetActive(false);
        };

        StartCoroutine(CooldownTextCoroutine());
        IEnumerator CooldownTextCoroutine()
        {
            var cooldown = skillDataSO.cooldown;

            while (cooldown > 0)
            {
                yield return new WaitForSeconds(1f);
                
                cooldown--;

                if (cooldown > 0)
                {
                    cooldownText.text = cooldown.ToString();   
                }
            }
        }
    }

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

    private void OnButtonClicked()
    {
        SkillManager.Instance.TryActiveSkill(skillDataSO);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}
