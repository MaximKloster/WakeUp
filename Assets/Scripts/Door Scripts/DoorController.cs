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
    bool openByEye = true, doubleDoor = false;

    [SerializeField]
    AudioClip soundOnClose = null, soundOnClosing = null;

    // Door variables
    string doorType;
    Transform doorTransform;
    float startAngle;
    bool open, openDoor, closeDoor;
    float angle;
    public bool OpenByEye { get { return openByEye; } }
    public bool OnAction
    {
        get
        {
            if (openDoor || closeDoor)
                return true;
            else
                return false;
        }
    }
    //float firstLookAt, lookToAktionTime = 2f;
    AudioSource doorSounds;

    // Use this for initialization
    void Start()
    {
        doorType = transform.name;
        doorTransform = transform.FindChild("Door Object");

        if (doorTransform.localRotation.eulerAngles.y > 180)
            startAngle = 360 - Mathf.Abs(doorTransform.localRotation.eulerAngles.y);
        else
            Mathf.Abs(doorTransform.localRotation.eulerAngles.y);

        doorSounds = gameObject.AddComponent<AudioSource>();
        doorSounds.playOnAwake = false;
        angle = startAngle;
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
                if (!doubleDoor)
                    doorTransform.transform.Rotate(-Vector3.up * SpeedToOpen);
                else
                    doorTransform.transform.Rotate(Vector3.up * SpeedToOpen);

                angle += SpeedToOpen;
                //doorTransform.transform.rotation = Quaternion.Lerp(doorTransform.transform.rotation,
                //    Quaternion.Euler(new Vector3(0, 360  + startPosition.rotation.eulerAngles.y - angleCompleteOpen, 0)),
                //    SpeedToOpen * Time.deltaTime);

                //if ((!doubleDoor && doorTransform.transform.rotation.eulerAngles.y <= 360 + startPosition.rotation.eulerAngles.y - angleCompleteOpen + 3)
                //    || (doubleDoor && doorTransform.transform.rotation.eulerAngles.y >= startPosition.rotation.eulerAngles.y - angleCompleteOpen - 3))
                if (angle >= angleCompleteOpen)
                {
                    openDoor = false;
                    open = true;
                    doorSounds.Stop();
                    eventCount--;
                    Debug.Log("Door open (ready) time: " + Time.time);
                }
            }
            //else if (doorType == "Slide Door")
            //{
            //    doorTransform.transform.position = Vector3.Lerp(doorTransform.transform.position,
            //        new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z + slideCompleteOpen),
            //        SpeedToOpen * Time.deltaTime);

            //    if (doorTransform.transform.position.z >= startPosition.position.z + slideCompleteOpen - 0.1f)
            //    {
            //        openDoor = false;
            //        open = true;
            //        doorSounds.Stop();
            //        eventCount--;
            //    }
            //}
        }
        // Close door
        else if (closeDoor)
        {
            if (doorType == "Door")
            {
                if (!doubleDoor)
                    doorTransform.transform.Rotate(Vector3.up * SpeedToClose);
                else
                    doorTransform.transform.Rotate(-Vector3.up * SpeedToClose);

                angle -= SpeedToClose;
                //doorTransform.transform.rotation = Quaternion.Lerp(doorTransform.transform.rotation,
                //    Quaternion.Euler(new Vector3(0, startPosition.rotation.eulerAngles.y, 0)),
                //    SpeedToClose * Time.deltaTime);

                //if ((!doubleDoor && doorTransform.transform.rotation.eulerAngles.y >= 180 - 3)
                //    || (doubleDoor && doorTransform.transform.rotation.eulerAngles.y <= startPosition.rotation.eulerAngles.y - 3))

                if (angle <= 0)
                {
                    closeDoor = false;
                    open = false;
                    if (soundOnClose)
                        PlaySoundOnClose();
                    Debug.Log("Door closed");
                }
            }
            //else if (doorType == "Slide Door")
            //{
            //    doorTransform.transform.position = Vector3.Lerp(doorTransform.transform.position,
            //        new Vector3(startPosition.transform.position.x, startPosition.transform.position.y, startPosition.transform.position.z),
            //        SpeedToClose * Time.deltaTime);

            //    if (doorTransform.transform.position.z <= startPosition.position.z + 0.01f)
            //    {
            //        closeDoor = false;
            //        open = false;
            //        if (soundOnClose)
            //            PlaySoundOnClose();
            //    }
            //}
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
        if (!open && !openDoor)
        {
            closeDoor = false;
            openDoor = true;

            Debug.Log("Door open time: " + Time.time);

            PlaySoundOnClosing();
        }
    }
    public void CloseDoor()
    {
        if (open && !closeDoor)
        {
            openDoor = false;
            closeDoor = true;

            PlaySoundOnClosing();
        }
    }

    public void LookAt()
    {
        if (openByEye)
        {
            if (!open)
                OpenDoor();
            else
                CloseDoor();
        }
    }
}
