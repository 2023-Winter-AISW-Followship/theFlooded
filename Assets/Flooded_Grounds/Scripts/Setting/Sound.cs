using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    [SerializeField]
    private AudioClip bottleExplosion;
    [SerializeField]
    private AudioClip radioNoise;
    [SerializeField]
    private AudioClip sparklerSparkle;

    [SerializeField]
    private AudioMixer audioMixer;
    private AudioMixerGroup itemMixer;

    public static Sound Instance { get; private set; }

    public static void BottleExplosion(Vector3 position)
    {
        Instance.itemSound(Instance.bottleExplosion, position, 1, 3, 50);
    }
    public static void RadioNoise(Vector3 position)
    {
        Instance.itemSound(Instance.radioNoise, position, 0.5f, 10, 40);
    }
    public static void SparklerSparkle(Vector3 position)
    {
        Instance.itemSound(Instance.sparklerSparkle, position, 1, 3, 15);
    }

    public void itemSound(AudioClip clip, Vector3 position, float volume, float min, float max)
    {
        GameObject gameObject = new GameObject("Item Sound");
        gameObject.transform.position = position;
        gameObject.layer = LayerMask.NameToLayer("sound");

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = Instance.itemMixer;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = min;
        audioSource.maxDistance = max;

        audioSource.Play();
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            itemMixer = audioMixer.FindMatchingGroups("item")[0];
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

//public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume)
//{
//    GameObject gameObject = new GameObject("One shot audio");
//    gameObject.transform.position = position;
//    AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
//    audioSource.clip = clip;
//    audioSource.spatialBlend = 1f;
//    audioSource.volume = volume;
//    audioSource.Play();
//    Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
//}