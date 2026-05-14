using UnityEngine;
using StarterAssets;
using Cinemachine;

public class ADS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("ADS Transform")]
    [SerializeField] private Vector3 adsPosition;
    [SerializeField] private Vector3 adsRotation;

    [Header("FOV")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float adsFOV = 30f;

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
    }

    private void Update()
    {
        if (input == null || weaponHolder == null || virtualCamera == null)
            return;

        bool isAiming = input.aim;

        // Move weapon
        weaponHolder.localPosition = Vector3.Lerp(
            weaponHolder.localPosition,
            isAiming ? adsPosition : normalPosition,
            Time.deltaTime * adsSpeed
        );

        weaponHolder.localRotation = Quaternion.Lerp(
            weaponHolder.localRotation,
            isAiming ? Quaternion.Euler(adsRotation) : normalRotation,
            Time.deltaTime * adsSpeed
        );

        // FOV zoom (THIS is the important part)
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
            virtualCamera.m_Lens.FieldOfView,
            isAiming ? adsFOV : normalFOV,
            Time.deltaTime * adsSpeed
        );
    }
}