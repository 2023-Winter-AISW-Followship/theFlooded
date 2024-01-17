using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    private AudioClip bottleExplosion;

    public static Sound Instance { get; private set; }

    public static AudioClip BottleExplosion
    {
        get { return Instance.bottleExplosion; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}