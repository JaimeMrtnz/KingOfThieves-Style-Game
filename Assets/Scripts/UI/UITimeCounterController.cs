using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the countdown in UI
/// </summary>
public class UITimeCounterController : MonoBehaviour
{
    private Text timeText;

    private void Awake()
    {
        timeText = GetComponent<Text>();

        GameEventManager.OnTimeChange += SetTime;
        GameEventManager.OnRetry += Reset;
    }

    private void OnDestroy()
    {
        GameEventManager.OnTimeChange -= SetTime;
        GameEventManager.OnRetry -= Reset;
    }

    /// <summary>
    /// Set the time in text
    /// </summary>
    /// <param name="secsLeft"></param>
    private void SetTime(ushort secsLeft)
    {
        if (secsLeft <= 10)
        {
            timeText.color = Color.red;
        }

        var t = TimeSpan.FromSeconds(secsLeft);
        timeText.text = t.ToString(@"mm\:ss");
    }

    /// <summary>
    /// Resets counter
    /// </summary>
    private void Reset()
    {
        timeText.color = Color.white;
    }
}
