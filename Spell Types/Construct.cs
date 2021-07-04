using UnityEngine;

//Spells that create an object in the world
public class Construct : ISpellType
{
    //Element type of the spell, defines certain behaviours of the spell e.g. fire spells will cause damage, earth spells will be more resistant
    Element.ElementType element = Element.ElementType.None;

    //Type of object to be spawned by the spell
    GameObject construct = null;

    //Reference to created construct, made global so that it can be accessed by all functions in the class
    GameObject newConstruct = null;

    //Spawn position for the construct in world space
    Vector3 spawnPosition = Vector3.zero;

    //Rotation of the object, allows the object to be spawned on different surfaces, will later be used to make object spawn relative to the direction the player is looking
    Quaternion spawnRotation = Quaternion.identity;

    //Size and lifespan (where relevent) of the construct
    float size = 1f, lifeSpan = 10f;

    //Reference to targetting pointer, all construct spells must be targetted in the world
    TargettingPointer targetpointer;

    //Create a new gameobject of the requested shape in the world, set its size and material and any elemental specific parameters/components
    public void Cast()
    {
        //Construct specific code
        newConstruct = GameObject.Instantiate(construct, spawnPosition, spawnRotation);
        Transform constructTransform = newConstruct.transform;
        constructTransform.localScale = new Vector3(size, size, size);
        Renderer constructRenderer = newConstruct.GetComponent<Renderer>();
        Material elementMaterial = Element.GetElementMaterial(element);
        constructRenderer.material = elementMaterial;

        //Element specific code
        ElementalProperties();

        Debug.LogFormat("Spell Type = Construct, Construct Object = {0}, Element = {1}, Spawn Position = {2}, Size = {3}, Life Span = {4}", construct.name, element, spawnPosition, size, lifeSpan);
    }

    //Set properties specific to spell element type
    public void ElementalProperties()
    {
        switch (element)
        {
            case Element.ElementType.Earth:
                //Eart constructs will remain forever if they are not damaged, no lifespan
                break;

            case Element.ElementType.Fire:
                //Fire can only be sustained for a certain amount of time
                TimedDestroy TD = newConstruct.AddComponent<TimedDestroy>();
                TD.SetLifeSpan(lifeSpan);
                break;

            case Element.ElementType.Ice:
                //Ice constructs should melt after a set period of time, may be affected by temperature in the future
                TD = newConstruct.AddComponent<TimedDestroy>();
                TD.SetLifeSpan(lifeSpan);
                break;

            default:
                break;
        }
    }

    //Public functions to set parameters
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

    //Set up targetting pointer parameters
    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
        construct.transform.localScale = new Vector3(size, size, size);
        targetpointer.SetTargetPrefab(construct);
    }

    //Function called during targetting so that targetting method is specific to the construct spell
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
