using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthData", menuName = "Game Data/Health Data")]
public class HealthData : ScriptableObject
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    [Header("Death Settings")]
    public float deathDelay = 2f;
}