using UnityEngine;

public class AreaOfEffect : ISpellType
{
    Element.ElementType element = Element.ElementType.None;
    GameObject spawnObjects = null;
    Vector3 center = Vector3.zero;
    float radius = 1, duration = 1;

    TargettingPointer targetpointer;
    GameObject AOEShape;

    public AreaOfEffect()
    {
        AOEShape = Resources.Load<GameObject>("Targetting/AOE Target");
    }

    public void Cast()
    {
        GameObject newAOE = GameObject.Instantiate(AOEShape, center, Quaternion.identity);
        Transform AOETransform = newAOE.transform;
        AOETransform.localScale = new Vector3(radius, AOETransform.localScale.y, radius);
        Renderer AOERenderer = newAOE.GetComponent<Renderer>();
        Material elementMaterial = Element.GetElementMaterial(element);
        AOERenderer.material = elementMaterial;
        Debug.LogFormat("Spell Type = AOE, Element = {0}, Center = {1}, Radius = {2}, Duration = {3}", element, center, radius, duration);
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void SetElement(Element.ElementType element)
    {
        this.element = element;
    }

    public void SetGameObject(GameObject go)
    {
        spawnObjects = go;
    }

    public void SetPosition(Vector3 position)
    {
        center = position;
    }

    public void SetRange(float range)
    {
        radius = range;
    }

    public void SetRotation(Quaternion rotation)
    {
        //No rotation for AOE, must be on flat surface
    }

    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
        AOEShape.transform.localScale = new Vector3(radius, AOEShape.transform.localScale.y, radius);
        targetpointer.SetTargetPrefab(AOEShape);
    }

    public bool Target()
    {
        bool validTarget = false;
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        validTarget = targetpointer.GetCurrentTargetPositioning(out position, out rotation);
        if (validTarget)
        {
            SetPosition(position);
        }
        return validTarget;
    }

    public override string ToString()
    {
        return "AOE";
    }
}
