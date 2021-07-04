using UnityEngine;

//Spells that create an effect in a flat area on a surface
public class AreaOfEffect : ISpellType
{
    //Element type of the spell, defines certain behaviours of the spawned spell e.g. ice will slow people down, fire will damage people etc.
    Element.ElementType element = Element.ElementType.None;

    //Some AOE spells may spawn smal objects in an area, e.g. spikes, this field holds a reference to that object
    GameObject spawnObjects = null;

    //Reference to spawned AOE object, made this global so that it can be accessed by all functions
    GameObject newAOE = null;

    //Center position in worldspace of the area
    Vector3 center = Vector3.zero;

    //Radius defiens the size of the area, duration defines how long the area lasts
    float radius = 1, duration = 2;

    //All AOE spells must have use object targetting to determine where the area will be created
    TargettingPointer targetpointer;

    //Game object that defines what the polygonal shape of the AOE spell looks like in the world, by default a circle but may want to have squares or triangles
    GameObject AOEShape;

    //Simple constructor to create a default AOE spell object and load the correct mesh from memory
    public AreaOfEffect()
    {
        AOEShape = Resources.Load<GameObject>("Targetting/AOE Target");
    }

    //Spawn a new area object in the world then set the size and material, set the lifespan of the area and any element specific parameters/components
    public void Cast()
    {
        //AOE specific code
        newAOE = GameObject.Instantiate(AOEShape, center, Quaternion.identity);
        Transform AOETransform = newAOE.transform;
        AOETransform.localScale = new Vector3(radius, AOETransform.localScale.y, radius);
        Renderer AOERenderer = newAOE.GetComponent<Renderer>();
        Material elementMaterial = Element.GetElementMaterial(element);
        AOERenderer.material = elementMaterial;
        TimedDestroy td = newAOE.AddComponent<TimedDestroy>();
        td.SetLifeSpan(duration);

        //Element specific code
        ElementalProperties();

        Debug.LogFormat("Spell Type = AOE, Element = {0}, Center = {1}, Radius = {2}, Duration = {3}", element, center, radius, duration);
    }

    //Run any addtional code that is specific to the element of the spell
    public void ElementalProperties()
    {
        switch (element)
        {
            case Element.ElementType.Earth:
                break;

            case Element.ElementType.Fire:
                break;

            case Element.ElementType.Ice:
                break;

            default:
                break;
        }
    }

    //Public functions to set parameters
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

    //Set up targetting for AOE
    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
        AOEShape.transform.localScale = new Vector3(radius, AOEShape.transform.localScale.y, radius);
        targetpointer.SetTargetPrefab(AOEShape);
    }

    //Function called during targetting so that targetting method is specific to the AOE spell
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
