using UnityEngine;

public class ShredderKillZone : MonoBehaviour
{
    public SmallShredder shredder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(shredder.smallItemTag))
        {
            shredder.ShredItem(other.gameObject);
        }
    }
}
