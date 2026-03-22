using UnityEngine;
using System.Collections.Generic;

public class SellPad : MonoBehaviour
{
    [SerializeField] private PlayerWallet wallet;

    private HashSet<Rigidbody> soldThisFrame = new HashSet<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Prevent double-selling from multiple child colliders
        if (soldThisFrame.Contains(rb)) return;
        soldThisFrame.Add(rb);

        ItemValue item = rb.GetComponent<ItemValue>();
        if (item != null)
        {
            wallet.AddMoney(item.value);
            Destroy(rb.gameObject);
        }
    }

    private void LateUpdate()
    {
        // Clear for the next frame
        soldThisFrame.Clear();
    }
}
