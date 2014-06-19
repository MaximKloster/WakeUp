using UnityEngine;
using System.Collections;

public class DoorTrigger_Modifired : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (transform.name == "Door Open Trigger")
                transform.parent.GetComponent<DoorController_Modifired>().OpenDoor();
            else if (transform.name == "Door Close Trigger")
                transform.parent.GetComponent<DoorController_Modifired>().CloseDoor();
    }
}
