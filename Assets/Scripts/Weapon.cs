using UnityEngine;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerCombatController combatController;

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
        if (starterAssetsInputs.shoot)
        {
            if (combatController != null && combatController.IsSwitching) return; // Do not shoot while switching weapons

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

            // Reset shoot to false OUTSIDE the raycast if-block
            starterAssetsInputs.ShootInput(false);
            // ShootInput(false) must be OUTSIDE the inner raycast if-block but INSIDE the outer shoot check. If you put it inside the raycast block, aiming at empty sky will leave shoot stuck as true forever � the gun fires every frame until it hits something.
        }
        
    }
}
