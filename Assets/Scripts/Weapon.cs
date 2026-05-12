using UnityEngine;
using StarterAssets;

public class Weapon : MonoBehaviour
{
    [SerializeField] float damage = 25f;
    StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        // StarterAssetsInputs is on the PlayerCapsule parent
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            RaycastHit hit; //A raycast fires an invisible ray from an origin point in a direction and returns the first collider it intersects.

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.name);
                Health health = hit.collider.GetComponent<Health>();

                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }

            // Reset shoot to false OUTSIDE the raycast if-block
            starterAssetsInputs.ShootInput(false);
            // ShootInput(false) must be OUTSIDE the inner raycast if-block but INSIDE the outer shoot check. If you put it inside the raycast block, aiming at empty sky will leave shoot stuck as true forever � the gun fires every frame until it hits something.
        }
        
    }
}
