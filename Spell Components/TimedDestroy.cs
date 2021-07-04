using UnityEngine;

//Component that destroys attached gameobject after a set lifespan
public class TimedDestroy : MonoBehaviour
{
    //Number of seconds between object spawning and being destroyed
    [SerializeField]
    float lifeSpan = 1f;

    float startTime = 0f;

    //Can toggle off the countdown with this parameter
    [SerializeField]
    bool countdown = true;

    //Come back to this, could be necessary to alert systems that the object has been destroyed
    public event TimedDestroyEvent DestroyEvent;
    public delegate void TimedDestroyEvent();

    void Awake()
    {
        startTime = Time.time;
    }

    //If countdown is active check if the lifespan has elapsed since the start time, if so destroy the gameobject
    void Update()
    {
        if(Time.time > startTime + lifeSpan && countdown)
        {
            /*
             * Change this later to use the event
            if(DestroyEvent.GetInvocationList().Length > 0)
            {
                DestroyEvent.Invoke();
            }*/
            Destroy(gameObject);
        }
    }

    //Public funtion to set the lifespan and start counting down if the object starts with countdown deactivated
    public void SetLifeSpan(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
        startTime = Time.time;
        countdown = true;
    }
}
