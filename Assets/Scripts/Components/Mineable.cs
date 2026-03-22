using UnityEngine;

public class Mineable : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject resourcePrefab; // what drops when mined

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void MineHit()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            Break();
        }
    }

    void Break()
    {
        // Spawn resource
        if (resourcePrefab != null)
        {
            Instantiate(resourcePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
