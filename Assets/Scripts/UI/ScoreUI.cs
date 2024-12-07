using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform scoreContainer;

    private int score;

    private void Start()
    {
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }
    
    private void PlacementArea_OnAnyObjectsPaired()
    {
        score++;
        scoreText.text = score.ToString();

        scoreContainer.DOPunchScale(Vector3.one * 0.2f, 0.5f, 5);
    }
    
    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
    }
}
