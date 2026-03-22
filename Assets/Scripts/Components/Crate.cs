using UnityEngine;

public class Crate : MonoBehaviour
{
    [Header("Grid Settings")]
    public Transform originPoint;      // bottom-front-left corner of crate interior
    public int gridWidth = 3;
    public int gridDepth = 5;
    public int gridHeight = 3;
    public float slotSpacing = 0.15f;
    public Transform crateRootTransform;
    public bool IsHeld;

    [Header("Dumping")]
    public Transform dumpPoint;

    private Rigidbody[,,] slots;

    private void Awake()
    {
        slots = new Rigidbody[gridWidth, gridHeight, gridDepth];
    }

    // ---------------------------------------------------------
    // 1. DETECTION LAYER
    // ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        PickupItem item = rb.GetComponent<PickupItem>();
        if (item == null) return;

        if (item.isHeld)
            return;

        // Check tag on the Rigidbody root, not the collider
        if (!rb.CompareTag("SmallJunk") && !rb.CompareTag("MaterialBit"))
            return;

        TryAcceptItem(rb);
    }

    // ---------------------------------------------------------
    // 2. CONTAINER LOGIC LAYER
    // ---------------------------------------------------------
    private void TryAcceptItem(Rigidbody rb)
    {
        if (IsAlreadyStored(rb))
            return;

        if (TryFindFreeSlot(out int sx, out int sy, out int sz))
        {
            slots[sx, sy, sz] = rb;

            Vector3 localSlotPos =
                originPoint.localPosition +
                new Vector3(sz * slotSpacing, sy * slotSpacing, sx * slotSpacing); //z axis first, due to model rotation. Might normalize later to something else

            // Pass to state transition layer
            FreezeAndParent(rb, localSlotPos);
        }
    }

    private bool IsAlreadyStored(Rigidbody rb)
    {
        for (int y = 0; y < gridHeight; y++)
            for (int x = 0; x < gridWidth; x++)
                for (int z = 0; z < gridDepth; z++)
                    if (slots[x, y, z] == rb)
                        return true;

        return false;
    }

    private bool TryFindFreeSlot(out int sx, out int sy, out int sz)
    {
        for (int y = 0; y < gridHeight; y++)
            for (int x = 0; x < gridWidth; x++)
                for (int z = 0; z < gridDepth; z++)
                    if (slots[x, y, z] == null)
                    {
                        sx = x; sy = y; sz = z;
                        return true;
                    }

        sx = sy = sz = -1;
        return false;
    }

    // ---------------------------------------------------------
    // 3. STATE TRANSITION LAYER
    // ---------------------------------------------------------
    private void FreezeAndParent(Rigidbody rb, Vector3 localSlotPos)
    {
        // Disable all colliders on the object
        Collider[] cols = rb.GetComponentsInChildren<Collider>();


        // Disable physics. Stop velocity to prevent scaling issues
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Physics.SyncTransforms();

        foreach (var c in cols)
            c.enabled = false;

        // Parent and snap
        rb.transform.SetParent(crateRootTransform, false);
        rb.transform.localPosition = localSlotPos;
        rb.transform.localRotation = Quaternion.identity;
        rb.transform.localScale = Vector3.one;

        PickupItem item = rb.GetComponent<PickupItem>();
if (item != null)
    item.isHeld = false;

    }


    // ---------------------------------------------------------
    // DUMPING
    // ---------------------------------------------------------
    public void Dump()
    {
        for (int y = 0; y < gridHeight; y++)
            for (int x = 0; x < gridWidth; x++)
                for (int z = 0; z < gridDepth; z++)
                {
                    Rigidbody rb = slots[x, y, z];
                    if (rb != null)
                    {
                        // Unparent
                        rb.transform.SetParent(null);

                        // Re-enable colliders
                        Collider[] cols = rb.GetComponentsInChildren<Collider>();
                        foreach (var c in cols)
                            c.enabled = true;

                        // Re-enable physics
                        rb.isKinematic = false;
                        rb.linearVelocity = Vector3.zero;

                        // Drop at dump point
                        rb.transform.position = dumpPoint.position;

                        slots[x, y, z] = null;
                    }
                }
    }

}
