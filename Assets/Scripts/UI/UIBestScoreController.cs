using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBestScoreController : MonoBehaviour
{
    [SerializeField]
    private Text bestScoreText;

    void Start()
    {
        gameObject.SetActive(true);
        bestScoreText.text = PlayerPrefs.GetString("BestScore");
    }
}
