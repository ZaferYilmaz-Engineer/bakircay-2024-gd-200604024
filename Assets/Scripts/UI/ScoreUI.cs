using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;

    private void Start()
    {
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }
    
    private void PlacementArea_OnAnyObjectsPaired()
    {
        score++;
        scoreText.text = score.ToString();
    }
    
    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
    }
}
