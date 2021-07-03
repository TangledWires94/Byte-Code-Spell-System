using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField]
    float lifeSpan = 1f;

    float startTime = 0f;
    bool countdown = false;

    public event TimedDestroyEvent DestroyEvent;
    public delegate void TimedDestroyEvent();

    public TimedDestroy(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
    }

    void Start()
    {
        startTime = Time.time;
    }

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

    public void SetLifeSpan(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
        countdown = true;
    }
}
