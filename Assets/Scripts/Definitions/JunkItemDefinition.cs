using UnityEngine;

[CreateAssetMenu(fileName = "NewJunkItem", menuName = "Junkyard/Junk Item")]
public class JunkItemDefinition : ScriptableObject
{
    public string displayName;
    public string sizeClass; // Small, Medium, Large

    [System.Serializable]
    public class MaterialYield
    {
        public MaterialDefinition material;
        public int minAmount = 1;
        public int maxAmount = 3;
    }

    public MaterialYield[] yields;
}
