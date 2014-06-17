using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    // Serialize variables
    [SerializeField]
    [Range(0, 180)]
    float angleCompletOpen = 90;
    [SerializeField]
    float timeToOpen = 2.0f, timeToClose = 0.5f;

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
        Debug.Log("close door");
        transform.FindChild("Door Object").transform.rotation = Quaternion.Lerp(transform.FindChild("Door Object").transform.rotation, Quaternion.Euler(new Vector3(270, 0, 0)), timeToClose * Time.deltaTime);
    }
}
