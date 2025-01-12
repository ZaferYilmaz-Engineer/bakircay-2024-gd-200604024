using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScoreSkill : BaseSkill
{
    protected override void HandleSkill()
    {
        GameplayUI.Instance.SetDoubleScoreState(true);
    }

    protected override void DeactivateSkill()
    {
        base.DeactivateSkill();
        GameplayUI.Instance.SetDoubleScoreState(false);
    }
}
