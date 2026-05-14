using UnityEngine;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    [Header("Ammo")]
    public int currentAmmo = 10;
    public int maxAmmo = 60;

    private StarterAssetsInputs starterAssetsInputs;
    private PlayerCombatController combatController;

    [Header("Weapon Settings")]
    public bool isAutomatic = false;
    public float fireRate = 5f; // shots per second

    private float nextTimeToFire = 0f;

    private void Awake()
    {
        // StarterAssetsInputs is on the PlayerCapsule parent
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatController = GetComponentInParent<PlayerCombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (starterAssetsInputs == null) return;

        if (!starterAssetsInputs.shoot) return;

        if (combatController != null && combatController.IsSwitching) return; // Do not shoot while switching weapons

        if (!CanShoot())
        {
            if (!isAutomatic)
            {
                starterAssetsInputs.ShootInput(false);
            }

            return;
        }

        Shoot();

        // Reset shoot to false OUTSIDE the raycast if-block
        if (!isAutomatic)
        {
            starterAssetsInputs.ShootInput(false);
        }

        // ShootInput(false) must be OUTSIDE the inner raycast if-block but INSIDE the outer shoot check. If you put it inside the raycast block, aiming at empty sky will leave shoot stuck as true forever — the gun fires every frame until it hits something.
        // For automatic weapons, we do NOT reset shoot here, because holding the button should keep firing.
        // For pistol / single fire weapons, we DO reset shoot here, because one click should only shoot once.
    }

    private void Shoot()
    {
        if (!UseAmmo(1))
        {
            Debug.Log(gameObject.name + " is out of ammo!");
            starterAssetsInputs.ShootInput(false);
            return;
        }

        nextTimeToFire = Time.time + (1f / fireRate);

        RaycastHit hit; //A raycast fires an invisible ray from an origin point in a direction and returns the first collider it intersects.
        AudioManager.Instance.PlayGunShot();

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, weaponData.range))
        {
            Debug.Log(hit.collider.name);
            Health health = hit.collider.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(weaponData.damage);
            }
        }
    }

    public bool AddAmmo(int amount)
    {
        if (currentAmmo >= maxAmmo) return false;

        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
        Debug.Log(gameObject.name + " ammo: " + currentAmmo + " / " + maxAmmo);

        return true;
    }

    private bool UseAmmo(int amount)
    {
        if (currentAmmo < amount) return false;

        currentAmmo -= amount;
        Debug.Log(gameObject.name + " ammo: " + currentAmmo + " / " + maxAmmo);

        return true;
    }

    public bool CanShoot()
    {
        return Time.time >= nextTimeToFire;
    }
}