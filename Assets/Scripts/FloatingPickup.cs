using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.25f;
    [SerializeField] private float rotateSpeed = 60f;

    [Header("Pickup")]
    [SerializeField] private string itemName;
    [SerializeField] private int amount = 1;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + Vector3.up * yOffset;
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupLogic grab = other.GetComponent<PickupLogic>();

        if (grab != null)
        {
            grab.AddItem(itemName, amount);
            Destroy(gameObject);
        }
    }
}