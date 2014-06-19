using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour
{
    [SerializeField]
    AudioSource eventSounds;

    [SerializeField]
    int eventCount = 1;

    bool eventActive = false;

    // Update is called once per frame
    void Update()
    {
        if (eventCount == 0) transform.FindChild("Event Activate Trigger").active = false;

        if (eventActive && !eventSounds.isPlaying)
        {
            eventSounds.Play();
            eventActive = false;
            eventCount--;
        }

    }

    public void ActivateEvent()
    {
        eventActive = true;
    }
}
