using UnityEngine;
using System.Collections;

public class LightTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (transform.name == "Light Flicker Trigger")
                transform.parent.GetComponent<NeonlightController>().Flicker = true;
            else if (transform.name == "Light Disable Trigger")
                transform.parent.GetComponent<NeonlightController>().On = false;
            else if (transform.name == "Light Disable Series Trigger")
                transform.parent.GetComponent<NeonlightController>().DisableSeries();
    }
}
