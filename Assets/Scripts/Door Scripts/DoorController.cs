using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    // Serialize variables
    [SerializeField]
    [Range(0, 180)]
    float angleCompletOpen = 90;
    [SerializeField]
    float timeToOpen = 2.0f;

    // Door variables
    float angleStartPosition;

    // Use this for initialization
    void Start()
    {
        angleStartPosition = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoor()
    {

    }
    public void CloseDoor()
    {

    }
}
