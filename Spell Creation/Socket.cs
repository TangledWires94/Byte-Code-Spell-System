using UnityEngine;

//Class to control sockets that user can drop components into
[RequireComponent(typeof(Collider2D))]
public class Socket : MonoBehaviour
{
    //Reference to trigger component
    Collider2D trigger = null;

    private void Awake()
    {
        trigger = GetComponent<Collider2D>();
    }

    public void ComponentInsert(SpellComponent spellComp)
    {
        spellComp.transform.parent = transform;
        spellComp.transform.localPosition = Vector3.zero;
        trigger.enabled = false;
    }

    public void ComponentPickup(SpellComponent spellComp)
    {
        trigger.enabled = true;
        spellComp.transform.parent = null;
    }



}
