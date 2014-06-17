using UnityEngine;
using System.Collections;

public class SwitchObjectTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            transform.parent.GetComponent<SwitchObjectController>().SwitchPosition();
    }
}
