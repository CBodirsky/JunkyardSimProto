using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    [Header("Mining Settings")]
    public GameObject outputPrefab;   // The ore prefab to spawn
    public float dropInterval = 2f;   // Seconds between drops

    [Header("Output Position")]
    public Transform dropPoint;       // Where the ore appears

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= dropInterval)
        {
            timer = 0f;
            DropOre();
        }
    }

    void DropOre()
    {
        if (outputPrefab != null && dropPoint != null)
        {
            GameObject ore = Instantiate(outputPrefab, dropPoint.position, dropPoint.rotation);

            // Apply random rotation
            ore.transform.rotation = Random.rotation;
        }
    }

}
