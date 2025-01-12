using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance { get; private set; }

    [HideInInspector] public bool isDoubleScoreActive;
    
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI doubleScoreText;
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
        doubleScoreText.gameObject.SetActive(false);
        scoreText.text = Score.ToString();
        PlacementArea.OnAnyObjectsPaired += PlacementArea_OnAnyObjectsPaired;
    }

    private void Update()
    {
        if (!isDoubleScoreActive)
        {
            return;
        }
        
        float r = Mathf.PingPong(Time.time, 1f);
        float g = Mathf.PingPong(Time.time * 0.8f, 1f);
        float b = Mathf.PingPong(Time.time * 0.6f, 1f);
        
        doubleScoreText.color = new Color(r, g, b);
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

    public void SetDoubleScoreState(bool _isDoubleScoreActive)
    {
        isDoubleScoreActive = _isDoubleScoreActive;
        doubleScoreText.gameObject.SetActive(isDoubleScoreActive);

        if (!isDoubleScoreActive)
        {
            doubleScoreText.transform.DOKill();
            return;
        }

        doubleScoreText.transform.localScale = Vector3.one;
        doubleScoreText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
    
    private void OnDestroy()
    {
        PlacementArea.OnAnyObjectsPaired -= PlacementArea_OnAnyObjectsPaired;
        resetButton.onClick.RemoveListener(OnResetButtonClicked);
    }
}
