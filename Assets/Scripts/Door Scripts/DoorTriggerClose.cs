using UnityEngine;
using System.Collections;

public class DoorTriggerClose : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            transform.parent.GetComponent<DoorController>().CloseDoor();
    }
}
