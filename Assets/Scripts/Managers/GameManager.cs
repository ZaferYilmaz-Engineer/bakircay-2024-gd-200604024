using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 144;
#endif
    }
}
