using UnityEngine;
using System.Collections;

public class AttackEvent : MonoBehaviour
{
    Transform player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.transform;
            StartCoroutine("Attack");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            player.GetComponent<WheelchairController>().SetLifeStatus(-1);
        }
    }
}
