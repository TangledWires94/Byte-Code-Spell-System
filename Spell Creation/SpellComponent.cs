using UnityEngine;
using System.Collections.Generic;

//Class for Spell Component objects: objects that contain information to build up the spell
public class SpellComponent : MonoBehaviour
{
    //Reference to sockets that component is near, keeps track of multiple if overlapping with several, empty if not near any
    List<Socket> socketList = new List<Socket>();

    //Reference to socket that component is currently inserted into
    Socket currentSocket = null;

    //Sorting layers used when dragging and when dropped
    [SerializeField, Min(1)]
    int draggingOrder = 1, defaultOrder = 1;

    //Reference to sprite renderer
    [SerializeField]
    SpriteRenderer rend = null; 

    //Set reference to drag and drop events, check for starting socket and get sprite renderer reference
    private void Awake()
    {
        try
        {
            DragAndDrop DrgAndDrp = GetComponent<DragAndDrop>();
            DrgAndDrp.StartDrag += ComponentPickup;
            DrgAndDrp.StopDrag += ComponentDrop;
        } catch (System.NullReferenceException)
        {
            Debug.Log("No Drag and Drop component connected to SpellComponent object");
        }

        try
        {
            currentSocket = GetComponentInParent<Socket>();
        } 
        catch (System.ArgumentOutOfRangeException)
        {
            //Do nothing, this code is only here in case the scene starts with components already within sockets
        }

        //rend = GetComponent<SpriteRenderer>();
    }

    //Code to handle picking up component
    void ComponentPickup()
    {
        rend.sortingOrder = draggingOrder;
        try
        {
            currentSocket.ComponentPickup(this);
            currentSocket = null;
        }
        catch (System.NullReferenceException)
        {
            //Do nothing, component wans't in a socket when picked up
        }
    }

    //If the component near a socket, insert the component into the socket
    void ComponentDrop()
    {
        rend.sortingOrder = defaultOrder;
        if (socketList.Count > 0)
        {
            //Insert into last socket entered and remove reference to all other sockets
            currentSocket = socketList[socketList.Count - 1];
            currentSocket.ComponentInsert(this);
        }
    }

    //When component enters a socket trigger, keep a reference to that socket so that it can be dropped into it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Socket")
        {
            Socket newSocket = collision.GetComponent<Socket>();
            if (!socketList.Contains(newSocket))
            {
                socketList.Add(newSocket);
            }
        }
    }

    //When component leaves a socket trigger, remove the socket from the list of tracked sockets
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Socket")
        {
            Socket newSocket = collision.GetComponent<Socket>();
            if (socketList.Contains(newSocket))
            {
                socketList.Remove(newSocket);
            }
        }
    }

    //If this component is destroyed clear references to DragAndDrop events
    private void OnDestroy()
    {
        try
        {
            DragAndDrop DrgAndDrp = GetComponent<DragAndDrop>();
            DrgAndDrp.StartDrag -= ComponentPickup;
            DrgAndDrp.StartDrag -= ComponentDrop;
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("No Drag and Drop component connected to SpellComponent object");
        }
    }
}
