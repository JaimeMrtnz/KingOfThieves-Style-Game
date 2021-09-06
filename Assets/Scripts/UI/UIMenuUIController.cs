using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject bestScorePanel;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScorePanel.SetActive(true);
        }
        else
        {
            bestScorePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Event handler for play button
    /// </summary>
    public void OnPlayClick()
    {
        SceneManager.LoadScene(1);
    }
}
