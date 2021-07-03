using UnityEngine;

//Static class to define all functionality regarding different spell properties e.g. materials
public static class Element
{
    public enum ElementType { None, Fire, Ice, Earth };

    public static Material GetElementMaterial(ElementType element)
    {
        Material elementMaterial = default;
        switch (element)
        {
            case ElementType.None:
                elementMaterial = Resources.Load<Material>("NullMaterial");
                break;
            case ElementType.Fire:
                elementMaterial = Resources.Load<Material>("FireMaterial");
                break;
            case ElementType.Ice:
                elementMaterial = Resources.Load<Material>("IceMaterial");
                break;
            case ElementType.Earth:
                elementMaterial = Resources.Load<Material>("EarthMaterial");
                break;
            default:
                elementMaterial = Resources.Load<Material>("NullMaterial");
                break;
        }
        return elementMaterial;

    }
}
