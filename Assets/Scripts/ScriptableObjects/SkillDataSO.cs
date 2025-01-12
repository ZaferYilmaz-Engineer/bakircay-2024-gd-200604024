using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkillDataSO", fileName = "Skill Data")]
public class SkillDataSO : ScriptableObject
{
    public float cooldown;
    public float duration;
}
