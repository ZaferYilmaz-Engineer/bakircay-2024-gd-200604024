using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;

    private bool isSkillActive;

    public void ActivateSkill()
    {
        if (isSkillActive)
        {
            return;
        }
        
        HandleSkill();
        Invoke(nameof(DeactivateSkill), skillDataSO.duration);
        isSkillActive = true;
    }

    protected abstract void HandleSkill();

    protected virtual void DeactivateSkill()
    {
        isSkillActive = false;
    }
}
