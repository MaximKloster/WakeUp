using UnityEngine;
using System.Collections;

public class GhostEvent : MonoBehaviour
{
    [SerializeField]
    float timeToEvent = 2f, ghostSpeed = 1f, followDistance = 3f;

    [SerializeField]
    AudioClip ghostSummonAudio, ghostSpawnAudio;

    Transform player, ghost, particles;
    Vector3 startPosition;
    NavMeshAgent navMeshAgent;
    bool startEvent;
    float eventStartTime;
    AudioSource audioSource;
    Animation patientAnimation;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        navMeshAgent = transform.GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
        ghost = transform.FindChild("Ghost Object");
        ghost.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);

        ghost.gameObject.SetActive(false);

        particles = transform.FindChild("Object").FindChild("Particles").transform;
        particles.gameObject.SetActive(false);
        patientAnimation = transform.FindChild("Object").GetComponentInChildren<Animation>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startEvent)// && eventStartTime + timeToEvent >= Time.time)
        {
            ghost.position = Vector3.Lerp(ghost.position, transform.position, ghostSpeed * Time.deltaTime);

            if (ghost.position.y >= transform.position.y - 0.1f)
            {
                ghost.position = transform.position;
                particles.gameObject.SetActive(false);
                startEvent = false;
                navMeshAgent.enabled = true;
                audioSource.clip = ghostSpawnAudio;
                audioSource.Play();
                print("foo");
            }
        }

        if (navMeshAgent.enabled && Vector3.Distance(navMeshAgent.transform.position, player.position) < followDistance)
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
                //navMeshAgent.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !startEvent && !navMeshAgent.enabled)
        {
            player = other.transform;
            ghost.gameObject.SetActive(true);
            particles.gameObject.SetActive(true);
            eventStartTime = Time.time;
            startEvent = true;
            patientAnimation.Play("summon");
            audioSource.clip = ghostSummonAudio;
            audioSource.Play();
        }
    }
}
