using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Combat")]
    public float damage = 25f;
    public float range = 100f;

    [Header("ADS")]
    public Vector3 adsPosition;
    public Vector3 adsRotation;
    public float normalFOV = 60f;
    public float adsFOV = 40f;
    public float adsSpeed = 10f;
}