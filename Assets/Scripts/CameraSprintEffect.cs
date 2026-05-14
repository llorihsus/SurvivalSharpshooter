using UnityEngine;

public class CameraSprintEffect : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float sprintOffsetZ = 0.2f;
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 originalLocalPos;
    private CharacterController controller;

    void Start()
    {
        originalLocalPos = cameraRoot.localPosition;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller == null) return;

        float speed = controller.velocity.magnitude;

        // Detect if running (adjust threshold if needed)
        bool isRunning = speed > 4f;

        Vector3 targetPos = originalLocalPos;

        if (isRunning)
        {
            targetPos += new Vector3(0, 0, sprintOffsetZ);
        }

        cameraRoot.localPosition = Vector3.Lerp(
            cameraRoot.localPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}