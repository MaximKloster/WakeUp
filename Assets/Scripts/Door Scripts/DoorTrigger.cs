using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (transform.name == "Door Open Trigger")
                transform.parent.GetComponent<DoorController>().OpenDoor();
            else if (transform.name == "Door Close Trigger")
                transform.parent.GetComponent<DoorController>().CloseDoor();
                
    }
}
