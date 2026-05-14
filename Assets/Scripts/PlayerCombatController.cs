using System.Collections;
using StarterAssets;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private Animator playerAnimator;

    [Header("Weapon Switching")]
    [SerializeField] private float switchSpeed = 8f;
    [SerializeField] private float loweredY = -0.5f;

    private StarterAssetsInputs input;
    private GameObject currentWeapon;
    private bool isSwitching = false;
    public bool IsSwitching => isSwitching;

    private void Start()
    {
        input = GetComponent<StarterAssetsInputs>();

        currentWeapon = weapon1;
        weapon1.SetActive(true);
        weapon2.SetActive(false);
    }

    private void Update()
    {
        if (input.melee)
        {
            Melee();
            input.melee = false;
        }

        if (input.weapon1)
        {
            StartWeaponSwitch(weapon1);
            input.weapon1 = false;
        }

        if (input.weapon2)
        {
            StartWeaponSwitch(weapon2);
            input.weapon2 = false;
        }
    }

    private void Melee()
    {
        Debug.Log("Melee");

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Melee");
        }
    }

    private void StartWeaponSwitch(GameObject newWeapon)
    {
        if (isSwitching) return;
        if (newWeapon == currentWeapon) return;

        StartCoroutine(SwitchWeaponRoutine(newWeapon));
    }

    private IEnumerator SwitchWeaponRoutine(GameObject newWeapon)
    {
        isSwitching = true;

        Transform oldTransform = currentWeapon.transform;
        Vector3 oldOriginalPos = oldTransform.localPosition;
        Vector3 oldLoweredPos = oldOriginalPos + new Vector3(0, loweredY, 0);

        while (Vector3.Distance(oldTransform.localPosition, oldLoweredPos) > 0.01f)
        {
            oldTransform.localPosition = Vector3.Lerp(
                oldTransform.localPosition,
                oldLoweredPos,
                Time.deltaTime * switchSpeed
            );

            yield return null;
        }

        oldTransform.localPosition = oldOriginalPos;
        currentWeapon.SetActive(false);

        currentWeapon = newWeapon;
        currentWeapon.SetActive(true);

        Transform newTransform = currentWeapon.transform;
        Vector3 newOriginalPos = newTransform.localPosition;
        Vector3 newLoweredPos = newOriginalPos + new Vector3(0, loweredY, 0);

        newTransform.localPosition = newLoweredPos;

        while (Vector3.Distance(newTransform.localPosition, newOriginalPos) > 0.01f)
        {
            newTransform.localPosition = Vector3.Lerp(
                newTransform.localPosition,
                newOriginalPos,
                Time.deltaTime * switchSpeed
            );

            yield return null;
        }

        newTransform.localPosition = newOriginalPos;
        isSwitching = false;
    }
}