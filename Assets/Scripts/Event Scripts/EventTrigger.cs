using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") transform.parent.GetComponent<EventController>().ActivateEvent();
    }
}
