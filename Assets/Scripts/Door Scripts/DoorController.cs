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
    bool openDoor, closeDoor;

    // Use this for initialization
    void Start()
    {
        angleStartPosition = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Open door
        if(openDoor)
            transform.FindChild("Door Object").transform.rotation = Quaternion.Lerp(transform.FindChild("Door Object").transform.rotation, Quaternion.Euler(new Vector3(270, angleStartPosition - angleCompletOpen, 0)), timeToClose * Time.deltaTime);
        // Close door
        else if (closeDoor)
        {
            transform.FindChild("Door Object").transform.rotation = Quaternion.Lerp(transform.FindChild("Door Object").transform.rotation, Quaternion.Euler(new Vector3(270, angleStartPosition, 0)), timeToClose * Time.deltaTime);
            if (transform.FindChild("Door Object").transform.rotation.eulerAngles.y <= angleStartPosition + 1)
            {
                closeDoor = false;

            }
        }
    }

    public void OpenDoor()
    {
        closeDoor = false;
        openDoor = true;
    }
    public void CloseDoor()
    {
        openDoor = false;
        closeDoor = true;

    }
}
