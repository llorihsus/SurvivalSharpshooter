using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [System.Serializable]
    public class LootEntry
    {
        public string itemName;
        public GameObject prefab;
        public float weight = 1f; // Higher = more likely
    }

    [SerializeField] private LootEntry[] lootTable;

    private bool hasDroppedLoot = false;

    public void DropLoot()
    {
        Debug.Log("DropLoot called on " + gameObject.name);
        if (hasDroppedLoot) return;
        hasDroppedLoot = true;

        float totalWeight = 0f;

        foreach (LootEntry entry in lootTable)
        {
            totalWeight += entry.weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (LootEntry entry in lootTable)
        {
            current += entry.weight;

            if (roll <= current)
            {
                // If prefab is empty, this means "drop nothing"
                if (entry.prefab != null)
                {
                    Vector3 spawnPos = transform.position;

                    RaycastHit hit;
                    if (Physics.Raycast(spawnPos, Vector3.down, out hit, 5f))
                    {
                        spawnPos = hit.point + Vector3.up * 0.2f;
                    }
                    else
                    {
                        spawnPos += Vector3.up * 0.5f;
                    }

                    GameObject obj = Instantiate(entry.prefab, spawnPos, Quaternion.identity);
                }

                return;
            }
        }
    }

    public void ResetLootDrop()
    {
        hasDroppedLoot = false;
    }
}