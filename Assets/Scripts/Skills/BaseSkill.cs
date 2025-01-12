using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public static event Action<float> OnAnySkillActivated;
    public static event Action<SkillDataSO> OnAnySkillInCooldown;
    
    public SkillDataSO skillDataSO;

    private bool isSkillActive;
    private bool isSkillOnCooldown;

    public void TryActivateSkill()
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
        OnAnySkillInCooldown?.Invoke(skillDataSO);
        yield return new WaitForSeconds(skillDataSO.cooldown);
        isSkillOnCooldown = false;
    }
}
