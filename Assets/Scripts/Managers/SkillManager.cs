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

    public void TryActiveSkill(int index)
    {
        if (!TouchManager.Instance.isTouchEnabled)
        {
            return;
        }
        
        if (index >= skillArray.Length || index < 0)
        {
            Debug.LogError("Skill index was out of bounds of skill array");
            return;
        }
        
        skillArray[index].ActivateSkill();
    }
}
