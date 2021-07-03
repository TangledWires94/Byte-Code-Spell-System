using UnityEngine;

public class Construct : ISpellType
{
    Element.ElementType element = Element.ElementType.None;
    GameObject construct = null;
    Vector3 spawnPosition = Vector3.zero;
    Quaternion spawnRotation = Quaternion.identity;
    float size = 1f, lifeSpan = 10f;

    TargettingPointer targetpointer;

    public void Cast()
    {
        //Create construct
        GameObject newConstruct = GameObject.Instantiate(construct, spawnPosition, spawnRotation);
        //Set size
        Transform constructTransform = newConstruct.transform;
        constructTransform.localScale = new Vector3(size, size, size);
        //Set material
        Renderer constructRenderer = newConstruct.GetComponent<Renderer>();
        Material elementMaterial = Element.GetElementMaterial(element);
        constructRenderer.material = elementMaterial;
        //Set lifespan
        TimedDestroy TD = newConstruct.AddComponent<TimedDestroy>();
        TD.SetLifeSpan(lifeSpan);

        Debug.LogFormat("Spell Type = Construct, Construct Object = {0}, Element = {1}, Spawn Position = {2}, Size = {3}, Life Span = {4}", construct.name, element, spawnPosition, size, lifeSpan);
    }

    public void SetDuration(float duration)
    {
        lifeSpan = duration;
    }

    public void SetElement(Element.ElementType element)
    {
        this.element = element;
    }

    public void SetGameObject(GameObject go)
    {
        construct = go;
    }

    public void SetPosition(Vector3 vector)
    {
        spawnPosition = vector;
    }

    public void SetRotation(Quaternion rotation)
    {
        spawnRotation = rotation;
    }

    public void SetRange(float range)
    {
        size = range;
    }

    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
        construct.transform.localScale = new Vector3(size, size, size);
        targetpointer.SetTargetPrefab(construct);
    }

    public bool Target()
    {
        //Debug.Log("Target");
        bool validTarget = false;
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        validTarget = targetpointer.GetCurrentTargetPositioning(out position, out rotation);
        //Debug.LogFormat("Position = {0}, Rotation = {1}", position, rotation);
        if (validTarget)
        {
            SetPosition(position);
            SetRotation(rotation);
        }
        return validTarget;
    }

    public override string ToString()
    {
        return "Construct";
    }
}
