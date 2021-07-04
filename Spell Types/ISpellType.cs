using UnityEngine;

//Interface that all types of spells (e.g. AOE, object creation, self-targetting etc.) must implement, allows VM to create and process spell objects of any type from a 
//bytecode file
public interface ISpellType
{
    void SetElement(Element.ElementType element);
    void SetGameObject(GameObject go);
    void SetPosition(Vector3 position);
    void SetRotation(Quaternion rotation);
    void SetRange(float range);
    void SetDuration(float duration);
    bool Target();
    void StartTargetting(TargettingPointer targetpointer);
    void Cast();
    void ElementalProperties();
}
