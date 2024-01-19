using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

[CreateAssetMenu(fileName ="Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string monsterName;
    public string MonsterName {  get { return monsterName; } }

    [SerializeField, Range(0f, 50f)]
    private int soundRecognitionDist;
    public int SoundRecognitionDist { get {  return soundRecognitionDist; } }

    [SerializeField, Range(0f, 70f)]
    private int sightRecognitionDist;
    public int SightRecognitionDist { get { return sightRecognitionDist; } }

    [SerializeField, Range(0f, 360f)]
    private int sightRecognitionAngle;
    public int SightRecognitionAngle { get { return sightRecognitionAngle; } }

    [SerializeField, Range(0f, 30f)]
    private int reach;
    public int Reach { get {  return reach; } }
}
