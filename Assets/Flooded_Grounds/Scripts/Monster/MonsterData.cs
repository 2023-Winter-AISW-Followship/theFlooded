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
    private int recognitionDist;
    public int RecognitionDist { get {  return recognitionDist; } }
    [SerializeField]
    private LayerMask recognitionTarget;
    public LayerMask RecognitionTarget { get { return recognitionTarget; } }
    [SerializeField]
    private int reach;
    public int Reach { get {  return reach; } }
}
