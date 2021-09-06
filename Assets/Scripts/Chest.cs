using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chest functionality
/// </summary>
public class Chest : SpawnableObject
{

    [SerializeField]
    private float degradationSpeed = 0.05f;

    private float currentLifetime;

    private bool ready = false;

    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (ready)
        {
            currentLifetime -= Time.deltaTime;

            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g - (1.0f / lifetime) * degradationSpeed,
                spriteRenderer.color.b - (1.0f / lifetime) * degradationSpeed,
                spriteRenderer.color.a
                );

            if (currentLifetime < 0)
            {
                Destroy(gameObject);
            } 
        }
    }

    /// <summary>
    /// Hack to initialize correctly the chest and avoid not initialized varaibles in Awake
    /// after the Instantiate 
    /// </summary>
    public void Ready()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLifetime = lifetime;
        ready = true;
    }
}
