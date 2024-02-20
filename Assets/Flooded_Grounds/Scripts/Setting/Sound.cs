using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    #region Sound Variable
    #region Item Sound Variable
    [SerializeField]
    private AudioClip bottleExplosion;
    [SerializeField]
    private AudioClip radioNoise;
    [SerializeField]
    private AudioClip sparklerSparkle;
    #endregion

    #region Step Sound Variable
    [SerializeField]
    private AudioClip[] footStep;

    public enum Type
    {
        Player,
        Monster,
        Item
    }
    #endregion

    #region Monster Sound Variable

    #endregion

    [SerializeField]
    private AudioMixer audioMixer;
    private AudioMixerGroup itemMixer;
    #endregion

    #region Item Sound
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
    #endregion

    #region Step Sound
    public static void FootStep(int i, Vector3 position, float volume, Type type)
    {
        Instance.StepSound(Instance.footStep[i], position, volume, type);
    }

    public void StepSound(AudioClip clip, Vector3 position, float volume, Type type)
    {
        GameObject gameObject = new GameObject("Step Sound");
        gameObject.transform.position = position;

        if (type == Type.Player)
        {
            gameObject.layer = LayerMask.NameToLayer("sound");

            BoxCollider boxCollider = (BoxCollider)gameObject.AddComponent(typeof(BoxCollider));
            boxCollider.isTrigger = true;
        }

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;

        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 100f;

        audioSource.Play();
        Destroy(gameObject, clip.length);
    }
    #endregion

    #region Monster Sound
    public static void HowlingSound(AudioClip clip, GameObject parent)
    {
        GameObject gameObject = new GameObject("Howling Sound");
        gameObject.transform.parent = parent.transform;
        gameObject.transform.localPosition = Vector3.zero;

        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;

        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 50f;
        audioSource.maxDistance = 100f;

        audioSource.Play();
        Destroy(gameObject, clip.length);
    }
    #endregion

    public static Sound Instance { get; private set; }

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