using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that can spawn in scene
/// </summary>
public class SpawnableObject : MonoBehaviour
{
    public float Lifetime { get => lifetime; set => lifetime = value; }


    public KeyValuePair<int, int> Index;

    protected float lifetime;

    private ParticleSystem takenParticles;

    private SpriteRenderer spriteRenderer;

    private Collider2D collider;

    private AudioSource audio;

    private bool alive = true;

    protected virtual void Awake()
    {
        takenParticles = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(takenParticles != null)
        {
            if(!alive && !takenParticles.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        else if(!alive)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks the object as taken by the player
    /// </summary>
    public void Taken()
    {
        alive = false;
        spriteRenderer.enabled = false;
        if (takenParticles != null)
        {
            takenParticles.Play();
        }

        if(audio != null)
        {
            audio.Play();
        }

        if(collider != null)
        {
            collider.enabled = false;
        }
    }
}
