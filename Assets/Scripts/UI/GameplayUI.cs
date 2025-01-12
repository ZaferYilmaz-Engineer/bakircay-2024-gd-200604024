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
    public static GameplayUI Instance { get; private set; }

    public bool isDoubleScoreActive;
    
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
        if (Instance)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }
    
    private void Start()
    {
        panel.SetActive(true);
        scoreText.text = Score.ToString();
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }

    private void OnResetButtonClicked()
    {
        Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    private void PlacementArea_OnAnyObjectsPaired(bool isSuccessful)
    {
        if (!isSuccessful)
        {
            return;
        }

        var addedScore = isDoubleScoreActive ? 2 : 1;
        
        Score += addedScore;
        scoreText.text = Score.ToString();

        scoreContainer.DOPunchScale(Vector3.one * 0.2f, 0.5f, 5);
    }
    
    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
        resetButton.onClick.RemoveListener(OnResetButtonClicked);
    }
}
