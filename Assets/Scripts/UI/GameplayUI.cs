using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform scoreContainer;
    [SerializeField] private Button resetButton;

    private int Score
    {
        get => PlayerPrefs.GetInt(nameof(Score), 0);
        set => PlayerPrefs.SetInt(nameof(Score), value);
    }

    private void Awake()
    {
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void OnResetButtonClicked()
    {
        Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        scoreText.text = Score.ToString();
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }
    
    private void PlacementArea_OnAnyObjectsPaired(bool isSuccessful)
    {
        if (!isSuccessful)
        {
            return;
        }
        
        Score++;
        scoreText.text = Score.ToString();

        scoreContainer.DOPunchScale(Vector3.one * 0.2f, 0.5f, 5);
    }
    
    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
        resetButton.onClick.RemoveListener(OnResetButtonClicked);
    }
}
