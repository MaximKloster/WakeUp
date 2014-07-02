using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    // Serialize variables
    [SerializeField]
    [Range(0, 180)]
    float angleCompleteOpen = 90, slideCompleteOpen = 1;

    [SerializeField]
    int eventCount = -1;

    [SerializeField]
    float SpeedToOpen = 2.0f, SpeedToClose = 0.5f;

    [SerializeField]
    bool doubleDoor;

    [SerializeField]
    AudioClip soundOnClose = null, soundOnClosing = null;

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

        if (doubleDoor)
            angleCompleteOpen *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventCount == 0) transform.FindChild("Door Open Trigger").gameObject.SetActive(false);

        //Open door
        if (openDoor)
        {
            if (doorType == "Door")
            {
                doorTransform.transform.rotation = Quaternion.Lerp(doorTransform.transform.rotation,
                    Quaternion.Euler(new Vector3(0, startPosition.rotation.eulerAngles.y - angleCompleteOpen, 0)),
                    SpeedToOpen * Time.deltaTime);

                if ((!doubleDoor && doorTransform.transform.rotation.eulerAngles.y <= startPosition.rotation.eulerAngles.y - angleCompleteOpen + 3)
                    || (doubleDoor && doorTransform.transform.rotation.eulerAngles.y >= startPosition.rotation.eulerAngles.y - angleCompleteOpen - 3))
                {
                    openDoor = false;
                    doorSounds.Stop();
                    eventCount--;
                }
            }
            else if (doorType == "Slide Door")
            {
                doorTransform.transform.position = Vector3.Lerp(doorTransform.transform.position,
                    new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z + slideCompleteOpen),
                    SpeedToOpen * Time.deltaTime);

                if (doorTransform.transform.position.z >= startPosition.position.z + slideCompleteOpen - 0.1f)
                {
                    openDoor = false;
                    doorSounds.Stop();
                    eventCount--;
                }
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

                if ((!doubleDoor && doorTransform.transform.rotation.eulerAngles.y >= startPosition.rotation.eulerAngles.y - 3)
                    || (doubleDoor && doorTransform.transform.rotation.eulerAngles.y <= startPosition.rotation.eulerAngles.y - 3))
                {
                    closeDoor = false;
                    if (soundOnClose)
                        PlaySoundOnClose();
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
                        PlaySoundOnClose();
                }
            }
        }
    }

    void PlaySoundOnClosing()
    {
        doorSounds.clip = soundOnClosing;
        doorSounds.Play();
    }
    void PlaySoundOnClose()
    {
        doorSounds.clip = soundOnClose;
        doorSounds.Play();
    }

    public void OpenDoor()
    {
        closeDoor = false;
        openDoor = true;

        PlaySoundOnClosing();
    }
    public void CloseDoor()
    {
        openDoor = false;
        closeDoor = true;

        PlaySoundOnClosing();
    }
}
