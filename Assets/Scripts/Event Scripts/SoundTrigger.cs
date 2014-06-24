using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") transform.parent.GetComponent<SoundController>().ActivateEvent();
    }
}
