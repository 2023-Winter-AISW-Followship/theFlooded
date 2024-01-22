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
    private AudioClip[] footStep;

    [SerializeField]
    private AudioMixer audioMixer;
    private AudioMixerGroup itemMixer;

    public static Sound Instance { get; private set; }

    public static void BottleExplosion(Vector3 position)
    {
        Instance.ItemSound(Instance.bottleExplosion, position, 3, 50);
    }
    public static void RadioNoise(Vector3 position)
    {
        Instance.ItemSound(Instance.radioNoise, position, 10, 40);
    }
    public static AudioClip SparklerSparkle(Vector3 position)
    {
        Instance.ItemSound(Instance.sparklerSparkle, position, "sparkler", 3, 15);
        return Instance.sparklerSparkle;
    }

    public static void FootStep(int i, Vector3 position, float volume)
    {
        Instance.StepSound(Instance.footStep[i], position, volume);
    }

    public void ItemSound(AudioClip clip, Vector3 position, string tag, float volume, float pitch, int min, int max)
    {
        GameObject gameObject = new GameObject("Item Sound");
        gameObject.transform.position = position;
        gameObject.tag = tag;
        gameObject.layer = LayerMask.NameToLayer("sound");

        BoxCollider boxCollider = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
        boxCollider.isTrigger = true;

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.outputAudioMixerGroup = Instance.itemMixer;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = min;
        audioSource.maxDistance = max;

        audioSource.Play();
        Destroy(gameObject, clip.length);
    }
    public void ItemSound(AudioClip clip, Vector3 position, string tag, int min, int max)
    {
        ItemSound(clip, position, tag, 1, 1, min, max);
        return;
    }
    public void ItemSound(AudioClip clip, Vector3 position, int min, int max)
    {
        ItemSound(clip, position, "Untagged", 1, 1, min, max);
        return;
    }
    public void ItemSound(AudioClip clip, Vector3 position)
    {
        ItemSound(clip, position, "Untagged", 1, 1, 10, 100);
        return;
    }

    public void StepSound(AudioClip clip, Vector3 position, float volume)
    {
        GameObject gameObject = new GameObject("Step Sound");
        gameObject.transform.position = position;
        gameObject.layer = LayerMask.NameToLayer("sound");

        BoxCollider boxCollider = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
        boxCollider.isTrigger = true;

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;

        audioSource.Play();
        Destroy(gameObject, clip.length);
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