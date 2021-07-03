using UnityEngine;

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
}
