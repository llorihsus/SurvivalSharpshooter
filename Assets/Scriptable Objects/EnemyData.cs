using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackDelay = 1.4f;
    public float attackRangeBonus = 1f;
}