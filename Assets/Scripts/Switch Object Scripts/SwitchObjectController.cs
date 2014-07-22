using UnityEngine;
using System.Collections;

public class SwitchObjectController : MonoBehaviour
{
    // variables
    [SerializeField]
    GameObject moveableObject = null;
    [SerializeField]
    bool loadAtStart = true;

    bool objectLoaded = false;
    Transform startPosition, secondPosition, thirdPosition, switchObject;
    GameObject controllerObject;

    // Use this for initialization
    void Start()
    {
        //initiate the other positions and the switch Object
        switchObject = transform.FindChild("Switch Object").transform;
        startPosition = transform.FindChild("Start Position").transform;
        secondPosition = transform.FindChild("Second Position").transform;
        thirdPosition = transform.FindChild("Third Position").transform;

        //initiate the moveable Object and set at the Start Position
        switchObject.position = startPosition.position;
        if (loadAtStart) loadObject();
    }

    private void loadObject()
    {
        if (moveableObject)
            controllerObject = Instantiate(moveableObject, startPosition.position, startPosition.rotation) as GameObject;
        controllerObject.transform.parent = transform.FindChild("Switch Object");
        objectLoaded = true;
    }

    public void SwitchPosition()
    {
        if (!objectLoaded)
        {
            //Debug.Log("loading Object");
            loadObject();
        }
        else if (switchObject.position == startPosition.position)
        {
            //Debug.Log("loading second Position");
            switchObject.position = secondPosition.position;
            switchObject.rotation = secondPosition.rotation;
        }
        else if (switchObject.position == secondPosition.position)
        {
            //Debug.Log("loading third Position");
            switchObject.position = thirdPosition.position;
            switchObject.rotation = thirdPosition.rotation;
        }
        else
        {
            //Debug.Log("back to start Position");
            switchObject.position = startPosition.position;
            switchObject.rotation = startPosition.rotation;
        }
    }

}
