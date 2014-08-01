using UnityEngine;
using System.Collections;

public class AttackEvent : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // leben abziehen
            // animation abspielen

        }
    }
}
