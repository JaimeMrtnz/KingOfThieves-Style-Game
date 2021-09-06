using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sensor to detect coins and chests
/// </summary>
public class ObjectsSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            GameManager.Instance.CoinTaken(other.gameObject.GetComponent<SpawnableObject>());
        }
        else if(other.gameObject.tag == "Chest")
        {
            GameManager.Instance.ChestTaken(other.gameObject.GetComponent<SpawnableObject>());
        }
    }
}
