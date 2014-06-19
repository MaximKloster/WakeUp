using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    // Serialize variables
    [SerializeField]
    [Range(0, 180)]
    float angleCompleteOpen = 90, slideCompleteOpen = 1;

    [SerializeField]
    float SpeedToOpen = 2.0f, SpeedToClose = 0.5f;

    [SerializeField]
    AudioClip soundOnClose, soundOnClosing;

    // Door variables
    string doorType;
    Transform doorTransform;
    Transform startPosition;
    bool openDoor, closeDoor;
    AudioSource doorSounds;

    // Use this for initialization
    void Start()
    {
        doorType = transform.name;
        doorTransform = transform.FindChild("Door Object");
        startPosition = transform;
        doorSounds = gameObject.AddComponent<AudioSource>();
        doorSounds.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Open door
        if (openDoor)
        {
            if (doorType == "Door")
            {
                doorTransform.transform.rotation = Quaternion.Lerp(doorTransform.transform.rotation,
                    Quaternion.Euler(new Vector3(0, startPosition.rotation.eulerAngles.y - angleCompleteOpen, 0)),
                    SpeedToOpen * Time.deltaTime);

                if (doorTransform.transform.rotation.eulerAngles.y <= startPosition.rotation.eulerAngles.y - angleCompleteOpen + 1)
                    openDoor = false;
            }
            else if (doorType == "Slide Door")
            {
                doorTransform.transform.position = Vector3.Lerp(doorTransform.transform.position,
                    new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z + slideCompleteOpen),
                    SpeedToOpen * Time.deltaTime);

                if (doorTransform.transform.position.z >= startPosition.position.z + slideCompleteOpen - 0.1f)
                    openDoor = false;
            }
        }
        // Close door
        else if (closeDoor)
        {
            if (doorType == "Door")
            {
                doorTransform.transform.rotation = Quaternion.Lerp(doorTransform.transform.rotation,
                    Quaternion.Euler(new Vector3(0, startPosition.rotation.eulerAngles.y, 0)),
                    SpeedToClose * Time.deltaTime);

                if (doorTransform.transform.rotation.eulerAngles.y >= startPosition.rotation.eulerAngles.y - 1)
                {
                    closeDoor = false;
                    if (soundOnClose)
                    {
                        doorSounds.clip = soundOnClose;
                        doorSounds.Play();
                    }
                }
            }
            else if (doorType == "Slide Door")
            {
                doorTransform.transform.position = Vector3.Lerp(doorTransform.transform.position,
                    new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z),
                    SpeedToClose * Time.deltaTime);

                if (doorTransform.transform.position.z <= startPosition.position.z + 0.01f)
                {
                    closeDoor = false;
                    if (soundOnClose)
                    {
                        doorSounds.clip = soundOnClose;
                        doorSounds.Play();
                    }
                }
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
