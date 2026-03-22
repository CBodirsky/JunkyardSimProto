using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactRange = 3f;
    public Transform holdPoint;
    public Transform crateRootTransform;

    private PickupItem heldItem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryMine();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
                TryPickup();
            else
                DropItem();
        }
    }

    void TryMine()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Mineable mineable = hit.collider.GetComponent<Mineable>();

            if (mineable != null)
            {
                mineable.MineHit();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            PickupItem item = hit.collider.GetComponentInParent<PickupItem>();

            if (item != null)
            {
                heldItem = item;
                heldItem.isHeld = true;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = false;
                }

                foreach (var col in heldItem.GetComponentsInChildren<Collider>())
                    col.enabled = false;

                heldItem.transform.SetParent(holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DropItem()
    {
        heldItem.isHeld = false;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();

        // 1. Re-enable colliders FIRST
        foreach (var col in heldItem.GetComponentsInChildren<Collider>())
            col.enabled = true;

        // 2. Re-enable physics
        rb.isKinematic = false;
        rb.detectCollisions = true;

        // 3. Force Unity to update the physics state
        rb.WakeUp();
        Physics.SyncTransforms();

        // 4. Unparent
        heldItem.transform.SetParent(null);

        heldItem = null;
    }

}
