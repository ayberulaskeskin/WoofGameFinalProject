using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;
    [SerializeField] GameObject _retryPanel;
    [SerializeField] GameObject _introPanel;
    [SerializeField] GameObject _nextLevelPanel;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Slider _powerBar;
    [SerializeField]  TextMeshProUGUI _scoreText;
    private void Awake()
    {
        manager = this;   

    }
    public void LoadNextLevel()
    {
        LevelManager.manager.NextLevel();
       
    }
    public void RestartLevel()
    {
        LevelManager.manager.Retry();
    }
    public void RetryMethod()
    {
        StartCoroutine(Retry());
    }
    IEnumerator Retry()
    {
        yield return new WaitForSeconds(1f);
        _retryPanel.SetActive(true);
    }
    public void ShowNextLevelPanel()
    {
        _nextLevelPanel.SetActive(true);
    }
    public void ProgressBar(float value)
    {
        _progressBar.value = value;
    }
    public void PowerBar(float value)
    {
        _powerBar.value = value;
    }
    public void HideIntro()
    {
        _introPanel.SetActive(false);
    }
    public void ScoreUpdate(int score)
    {
        _scoreText.text = "" + score;
    }
}