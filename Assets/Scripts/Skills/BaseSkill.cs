using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public static event Action<float> OnAnySkillActivated;
    
    [SerializeField] private SkillDataSO skillDataSO;

    private bool isSkillActive;
    private bool isSkillOnCooldown;

    public void ActivateSkill()
    {
        if (isSkillActive || isSkillOnCooldown)
        {
            return;
        }
        
        HandleSkill();
        
        OnAnySkillActivated?.Invoke(skillDataSO.duration);
        Invoke(nameof(DeactivateSkill), skillDataSO.duration);
        isSkillActive = true;
    }

    protected abstract void HandleSkill();

    protected virtual void DeactivateSkill()
    {
        isSkillActive = false;
        StartCoroutine(HandleCooldown());
    }

    private IEnumerator HandleCooldown()
    {
        isSkillOnCooldown = true;
        yield return new WaitForSeconds(skillDataSO.cooldown);
        isSkillOnCooldown = false;
    }
}
