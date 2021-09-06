using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class that manages Inputs and invokes their events
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public delegate void ClickEvent();

    public event ClickEvent OnTap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnTap?.Invoke();
        }
    }
}
