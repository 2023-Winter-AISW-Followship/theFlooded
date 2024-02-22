using UnityEngine;

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

    [SerializeField, Range(0f, 8f)]
    private int speed;
    public int Speed { get { return speed; } }

    [SerializeField, Range(0f, 15f)]
    private int runSpeed;
    public int RunSpeed { get { return runSpeed; } }

    [SerializeField]
    private Vector3 hitSize;
    public Vector3 HitSize { get {  return hitSize; } }

    [SerializeField]
    private AudioClip howlingSound;
    public AudioClip HowlingSound { get { return howlingSound; } }

    [SerializeField, Range(0f, 100f)]
    private float damage;
    public float Damage { get { return damage; } }

}
