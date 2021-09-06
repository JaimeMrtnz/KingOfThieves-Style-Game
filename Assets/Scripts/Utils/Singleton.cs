

using UnityEngine;

/// <summary>
/// Parameterized Singleton class
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<T>();
            }
            else
            {
                return instance;
            }
        }
    }
} 
