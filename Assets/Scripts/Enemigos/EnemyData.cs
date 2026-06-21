using UnityEngine;
public enum EnemyType
    {
        Melee,
        Ranged,
        Tank,
        Boss,
        Spectrum
    }

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType = EnemyType.Melee;
    public float maxHealth;
    public float speed;
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public float visionRange;
    public float walkHearingRange;
    public float runHearingRange;
    public float hearingMessageRange;
    public float angleVisionH;
    public float angleVisionV;
    public float frecuencySerpenteo=0f;
}
