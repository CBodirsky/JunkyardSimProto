using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "Junkyard/Material")]
public class MaterialDefinition : ScriptableObject
{
    public string materialName;

    public Color displayColor;

    [Header("Value")]
    public int baseValue = 1; // value per bit
    public float refinedMultiplier = 1.5f; // smelted/ingot value boost

    [Header("Prefabs")]
    public GameObject bitPrefab;
    public GameObject ingotPrefab;
    public GameObject blockPrefab;
}
