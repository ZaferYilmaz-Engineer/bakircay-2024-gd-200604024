using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Button firstSkillButton;
    [SerializeField] private Button secondSkillButton;

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
    }
}
