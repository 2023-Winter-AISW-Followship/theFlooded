using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monserData : ScriptableObject
{
    [SerializeField]
    private string monsterName;
    public string MonsterName {  get { return monsterName; } }
    [SerializeField]
    private float recognitionDist;
    public float RecognitionDist { get {  return recognitionDist; } }
    [SerializeField]
    private LayerMask recognitionTarget;
    public LayerMask RecognitionTarget { get { return recognitionTarget; } }

}
