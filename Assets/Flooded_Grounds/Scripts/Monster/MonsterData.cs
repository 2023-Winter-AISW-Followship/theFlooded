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
    [SerializeField]
    private int soundRecognitionDist;
    public int SoundRecognitionDist { get {  return soundRecognitionDist; } }
    [SerializeField]
    private int sightRecognitionDist;
    public int SightRecognitionDist { get { return sightRecognitionDist; } }
    [SerializeField]
    private int reach;
    public int Reach { get {  return reach; } }
}
