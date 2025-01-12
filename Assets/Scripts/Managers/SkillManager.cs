using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    
    [SerializeField] private BaseSkill[] skillArray;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void TryActiveSkill(SkillDataSO skillDataSO)
    {
        if (!TouchManager.Instance.isTouchEnabled)
        {
            return;
        }

        foreach (var skill in skillArray)
        {
            if (skill.skillDataSO != skillDataSO) continue;
            
            skill.TryActivateSkill();
            break;
        }
    }
}
