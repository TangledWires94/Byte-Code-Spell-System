using UnityEngine;

[RequireComponent(typeof(Transform), typeof(Collider2D))]
public class DragAndDrop : MonoBehaviour
{
    //State of the drag and drop process
    [SerializeField]
    enum DADState { Dropped, Dragging};
    [SerializeField]
    DADState state = DADState.Dropped;

    //Drag & Drop events
    public delegate void DragAndDropEvent();
    public DragAndDropEvent StartDrag, StopDrag;

    private void Awake()
    {
        state = DADState.Dropped;
    }

    private void Update()
    {
        if(state == DADState.Dragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(mousePosition);
            newPos.z = 0f;
            transform.position = newPos;

            if (Input.GetMouseButtonUp(0))
            {
                state = DADState.Dropped;
                try
                {
                    StopDrag.Invoke();
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("No subscribers to Stop Drag event");
                }
            }
        }
    }

    //Code called while mouse is over the object collider
    private void OnMouseOver()
    {
        if(state == DADState.Dropped)
        {
            //If the mouse is over the object and the user clicks the left mouse button, start dragging
            if (Input.GetMouseButtonDown(0))
            {
                state = DADState.Dragging;
                try
                {
                    StartDrag.Invoke();
                }
                catch (System.NullReferenceException)
                {
                    Debug.Log("No subscribers to Start Drag event");
                }
            }
        }
    }
}
