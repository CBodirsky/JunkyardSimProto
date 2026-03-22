using UnityEngine;
using System.Collections;

public class SmallShredder : MonoBehaviour
{
    [Header("References")]
    public Transform outputPoint;

    [Header("Settings")]
    public float pullForce = 10f;
    public string smallItemTag = "SmallJunk";

    [Header("Output")]
    public GameObject[] materialBitPrefabs;
    public int bitsPerItem = 3;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(smallItemTag)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Pull toward the center of the shredder
        Vector3 direction = (transform.position - other.transform.position).normalized;
        rb.AddForce(direction * pullForce, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(smallItemTag)) return;

        // If the object enters the KillZone, shred it
        if (other.gameObject.name.Contains("KillZone")) return;
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: handle exit logic if needed
    }

    public void ShredItem(GameObject item)
    {
        JunkItem junk = item.GetComponent<JunkItem>();
        if (junk == null)
        {
            Destroy(item);
            return;
        }

        StartCoroutine(SpawnBitsOverTime(junk.definition.yields));
        Destroy(item);
    }


    private IEnumerator SpawnBitsOverTime(JunkItemDefinition.MaterialYield[] yields)
    {
        foreach (var yield in yields)
        {
            GameObject bitPrefab = yield.material.bitPrefab;
            int amount = Random.Range(yield.minAmount, yield.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                GameObject bit = Instantiate(bitPrefab, outputPoint.position, Random.rotation);

                Rigidbody rb = bit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddForce(outputPoint.forward * 2f, ForceMode.Impulse);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    //private GameObject GetBitPrefab(JunkItem.MaterialType type)
    //{
    //    // For now, just return IronBit since that's all you have
    //    return materialBitPrefabs[0];
    //}

    //private IEnumerator SpawnBitsOverTime(JunkItem junk)
    //{
    //    for (int i = 0; i < junk.yieldAmount; i++)
    //    {
    //        GameObject bitPrefab = GetBitPrefab(junk.material);
    //        GameObject bit = Instantiate(bitPrefab, outputPoint.position, Random.rotation);

    //        Rigidbody rb = bit.GetComponent<Rigidbody>();
    //        if (rb != null)
    //        {
    //            rb.AddForce(outputPoint.forward * 2f, ForceMode.Impulse);
    //        }

    //        yield return new WaitForSeconds(0.1f); // tweak this for pacing
    //    }
    //}


}
