using UnityEngine;

[CreateAssetMenu(menuName = "Items/Material Item")]
public class MaterialItem : Item {
    public enum MaterialType
    {
        Wood,
        Other
    }

    public enum SubMaterialType
    {
        Oak,
        Birch,
        Iron,
        Steel,
        Amethyst,
        Granite,
        Stone
    }

    public MaterialType subItemType;
    public SubMaterialType materialType;
}
