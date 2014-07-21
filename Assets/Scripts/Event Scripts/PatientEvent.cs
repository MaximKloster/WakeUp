using UnityEngine;
using System.Collections;

public class PatientEvent : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    bool startEvent;
    Vector3 startPosition;
    Transform player;
    float eventStartTime;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        navMeshAgent = transform.GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startEvent && navMeshAgent.enabled && player)
        {
            navMeshAgent.destination = player.position;
        }
        else if (navMeshAgent.enabled)
        {
            navMeshAgent.destination = startPosition;

            if (navMeshAgent.transform.position.x > startPosition.x - 0.05f && navMeshAgent.transform.position.x < startPosition.x + 0.05f
                && navMeshAgent.transform.position.y > startPosition.y - 0.05f && navMeshAgent.transform.position.y < startPosition.y + 0.05f
                && navMeshAgent.transform.position.z > startPosition.z - 0.05f && navMeshAgent.transform.position.z < startPosition.z + 0.05f)
            {
                navMeshAgent.transform.position = startPosition;
                navMeshAgent.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !startEvent)
        {
            player = other.transform;
            startEvent = true;
            eventStartTime = Time.time;
            navMeshAgent.enabled = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && startEvent)
        {
            startEvent = false;
        }
    }
}
