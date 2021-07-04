using UnityEngine;

//Spells that target the user, e.g. buffs, healing, applying status effects etc.
public class SelfTarget : ISpellType
{
    Element.ElementType element = Element.ElementType.None;
    GameObject newForm = default;
    Vector3 newPlayerPosition = Vector3.zero;
    float range = 0, duration = 0;

    TargettingPointer targetpointer;

    public void Cast()
    {
        //Self-targetting specific code

        //Element specific code
        ElementalProperties();

        Debug.LogFormat("Spell Type = Self-Targetting, Element = {0}, New Position = {1}, Teleport Range = {2}, Duration = {3}", element, newPlayerPosition, range, duration);
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
        newForm = go;
    }

    public void SetPosition(Vector3 position)
    {
        newPlayerPosition = position;
    }

    public void SetRange(float range)
    {
        //Not used
    }

    public void SetRotation(Quaternion rotation)
    {
        //Not used
    }

    //Unlikely that any of the targetting functions will ever be used for this spell type, target is always the user so just need to return the player position and end 
    public void StartTargetting(TargettingPointer targetpointer)
    {
        this.targetpointer = targetpointer;
    }

    public bool Target()
    {
        //Self targetting so no position as it will effect player stats
        SetPosition(Vector3.zero);
        targetpointer.DisableTargetting();
        return true;
    }

    public override string ToString()
    {
        return "Self";
    }
}
