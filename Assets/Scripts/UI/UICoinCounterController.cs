using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoinCounterController : MonoBehaviour
{
    private Text coinAmountText;

    private void Awake()
    {
        coinAmountText = GetComponent<Text>();

        GameEventManager.OnCoinsTaken += SetCoin;
    }

    private void OnDestroy()
    {
        GameEventManager.OnCoinsTaken -= SetCoin;
    }

    /// <summary>
    /// Adds a coin to the counter
    /// </summary>
    private void SetCoin(byte coins)
    {
        coinAmountText.text = coins.ToString();
    }
}
