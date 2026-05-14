using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupLogic : MonoBehaviour
{
    [Header("Weapons")]
    public PlayerCombatController combatController;

    [Header("Sunscreen Shield")]
    public float sunscreenShield = 0f;
    public float maxSunscreenShield = 50f;

    [Header("UI")]
    public TMP_Text ammoText;
    public Image sunscreenBar;

    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();

        if (combatController == null)
        {
            combatController = GetComponentInChildren<PlayerCombatController>();
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddItem(string itemName, int amount)
    {
        switch (itemName)
        {
            case "Healing Potion":
                if (playerHealth != null)
                {
                    playerHealth.Heal(amount);
                }
                break;

            case "Sunscreen":
                sunscreenShield = Mathf.Min(maxSunscreenShield, sunscreenShield + amount);
                break;

            case "Ammo":
                Weapon activeWeapon = GetActiveWeapon();

                if (activeWeapon != null)
                {
                    activeWeapon.AddAmmo(amount);
                }
                break;
        }

        UpdateUI();
        Debug.Log("Picked up: " + itemName + " x" + amount);
    }

    public float AbsorbSunDamage(float damage)
    {
        float absorbedAmount = Mathf.Min(sunscreenShield, damage);
        sunscreenShield -= absorbedAmount;

        UpdateUI();

        return damage - absorbedAmount;
    }

    private Weapon GetActiveWeapon()
    {
        if (combatController == null)
        {
            combatController = GetComponentInChildren<PlayerCombatController>();
        }

        if (combatController == null)
        {
            return null;
        }

        Weapon[] weapons = combatController.GetComponentsInChildren<Weapon>(true);

        foreach (Weapon weapon in weapons)
        {
            if (weapon.gameObject.activeInHierarchy)
            {
                return weapon;
            }
        }

        return null;
    }

    private void UpdateUI()
    {
        Weapon activeWeapon = GetActiveWeapon();

        if (activeWeapon != null && ammoText != null)
        {
            ammoText.text = activeWeapon.currentAmmo + " / " + activeWeapon.maxAmmo;
        }

        if (sunscreenBar != null)
        {
            sunscreenBar.fillAmount = sunscreenShield / maxSunscreenShield;
        }
    }
}