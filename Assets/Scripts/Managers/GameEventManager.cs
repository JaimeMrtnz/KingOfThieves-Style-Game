using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Stores main events in the game
/// </summary>
public class GameEventManager : MonoBehaviour
{
    public delegate void Event();
    public static event Event OnMapLoaded;
    public static void MapLoaded()
    {
        OnMapLoaded?.Invoke();
    }

    public static event Event OnGameOver;
    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static event Event OnRetry;
    public static void Retry()
    {
        OnRetry?.Invoke();
    }

    public delegate void TimeEvent(ushort secsLeft);
    public static event TimeEvent OnTimeChange;
    public static void TimeChanged(ushort secsLeft)
    {
        OnTimeChange?.Invoke(secsLeft);
    }



    public delegate void CoinEvent(byte coins);
    public static event CoinEvent OnCoinsTaken;
    public static void CoinsTaken(byte coins)
    {
        OnCoinsTaken?.Invoke(coins);
    }
}
