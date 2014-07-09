using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}
