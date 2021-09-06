using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// controls the main UI
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject RetryButton;

    [SerializeField]
    private Text score;


    private void Awake()
    {   
        GameEventManager.OnGameOver += GameOver;    
    }

    private void OnDestroy()
    {
        GameEventManager.OnGameOver -= GameOver;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        RetryButton.SetActive(false);
    }

    /// <summary>
    /// shows a Game Over mesasge
    /// </summary>
    private void GameOver()
    {
        score.text = GameManager.Instance.Coins.ToString();
        gameOverPanel.SetActive(true);
        RetryButton.SetActive(true);
    }

    /// <summary>
    /// Event handler of Retry click button
    /// </summary>
    public void OnClickRetry()
    {
        gameOverPanel.SetActive(false);
        RetryButton.SetActive(false);

        GameEventManager.Retry();
    }
}
