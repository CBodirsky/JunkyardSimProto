using UnityEngine;

public class BuildMode : MonoBehaviour
{
    [Header("Build Settings")]
    public GameObject prefabToPlace;
    public LayerMask placementMask;
    public Camera playerCamera;

    private GameObject preview;
    private bool isBuilding = false;
    private Quaternion currentRotation = Quaternion.identity;
    private PlayerControls controls;


    //void Update()
    //{
    //    HandleToggle();

    //    if (isBuilding)
    //    {
    //        HandlePreview();
    //        HandleRotation();
    //        HandlePlacement();
    //    }
    //}

    void Update()
    {
        if (isBuilding)
        {
            HandlePreview();
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.BuildMode.performed += _ => ToggleBuildMode();
        controls.Player.CancelBuild.performed += _ => ExitBuildMode();
        controls.Player.Place.performed += _ => TryPlace();
        controls.Player.RotateLeft.performed += _ => RotateLeft();
        controls.Player.RotateRight.performed += _ => RotateRight();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    //void HandleToggle()
    //{
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        EnterBuildMode();
    //    }

    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        ExitBuildMode();
    //    }
    //}

    void ToggleBuildMode()
    {
        if (isBuilding) ExitBuildMode();
        else EnterBuildMode();
    }


    void EnterBuildMode()
    {
        if (isBuilding) return;

        isBuilding = true;
        preview = Instantiate(prefabToPlace);
        DisableColliders(preview);
        DisableScripts(preview);

        int previewLayer = LayerMask.NameToLayer("Preview");
        foreach (Transform t in preview.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = previewLayer;

        SetPreviewMaterial(preview, true);
    }

    void ExitBuildMode()
    {
        isBuilding = false;

        if (preview != null)
            Destroy(preview);
    }

    void HandlePreview()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool didHit = Physics.Raycast(ray, out hit, 100f, placementMask);

        if (didHit)
        {
            Vector3 snapped = Grid.SnapToGrid(hit.point);
            preview.transform.position = snapped;
            preview.transform.rotation = currentRotation;

            bool valid = CanPlaceHere();
            SetPreviewMaterial(preview, valid);
        }

        Debug.Log("Raycast hit: " + (didHit ? hit.collider.name : "NONE"));
        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red);

    }


    //void HandleRotation()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //        currentRotation *= Quaternion.Euler(0, -90, 0);

    //    if (Input.GetKeyDown(KeyCode.E))
    //        currentRotation *= Quaternion.Euler(0, 90, 0);
    //}

    void RotateLeft()
    {
        currentRotation *= Quaternion.Euler(0, -90, 0);
    }

    void RotateRight()
    {
        currentRotation *= Quaternion.Euler(0, 90, 0);
    }


    //void HandlePlacement()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Instantiate(prefabToPlace, preview.transform.position, preview.transform.rotation);
    //    }
    //}

    void TryPlace()
    {
        if (!isBuilding) return;
        if (!CanPlaceHere()) return;

        var placed = Instantiate(prefabToPlace, preview.transform.position, preview.transform.rotation);

        int placedLayer = LayerMask.NameToLayer("PlacedObject");
        foreach (Transform t in placed.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = placedLayer;
    }

    bool CanPlaceHere()
    {
        // Get the renderer bounds of the preview
        Renderer r = preview.GetComponentInChildren<Renderer>();
        if (r == null) return false;

        Vector3 pos = preview.transform.position;
        Vector3 halfExtents = r.bounds.extents * 0.9f; // shrink slightly to allow touching edges

        int mask = LayerMask.GetMask("PlacedObject");

        return !Physics.CheckBox(pos, halfExtents, preview.transform.rotation, mask);
    }


    void DisableColliders(GameObject obj)
    {
        Collider[] cols = obj.GetComponentsInChildren<Collider>(true);
        foreach (var col in cols)
            col.enabled = false;
    }

    void DisableScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var s in scripts)
            s.enabled = false;
    }



    void SetPreviewMaterial(GameObject obj, bool valid)
    {
        Color c = valid ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);

        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.color = c;
            }
        }
    }

}
