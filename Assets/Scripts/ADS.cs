using StarterAssets;
using UnityEngine;

public class ADS : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Camera playerCamera;

    [Header("ADS Transform")]
    [SerializeField] private Vector3 adsPosition;
    [SerializeField] private Vector3 adsRotation;

    [Header("FOV")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float adsFOV = 40f;

    [Header("Speed")]
    [SerializeField] private float adsSpeed = 10f;

    private Vector3 normalPosition;
    private Quaternion normalRotation;
    private StarterAssetsInputs input;

    private void Start()
    {
        input = GetComponent<StarterAssetsInputs>();

        normalPosition = weaponHolder.localPosition;
        normalRotation = weaponHolder.localRotation;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (input == null || weaponHolder == null || playerCamera == null) return;

        bool isAiming = input.aim;

        Vector3 targetPosition = isAiming ? adsPosition : normalPosition;
        Quaternion targetRotation = isAiming ? Quaternion.Euler(adsRotation) : normalRotation;

        weaponHolder.localPosition = Vector3.Lerp(
            weaponHolder.localPosition,
            targetPosition,
            Time.deltaTime * adsSpeed
        );

        weaponHolder.localRotation = Quaternion.Lerp(
            weaponHolder.localRotation,
            targetRotation,
            Time.deltaTime * adsSpeed
        );

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            isAiming ? adsFOV : normalFOV,
            Time.deltaTime * adsSpeed
        );
    }
}