using UnityEngine;
using System.Collections;

public class SwitchObjectController : MonoBehaviour
{
    // variables
    [SerializeField]
    Transform moveableObject = null;
    [SerializeField]
    GameObject parent;
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
        controllerObject = Instantiate(moveableObject, startPosition.position, startPosition.rotation) as GameObject;
        controllerObject.transform.parent = transform.FindChild("Switch Object");
    }

    public void SwitchPosition()
    {
        Debug.Log("switching...");
        if (switchObject.position == startPosition.position)
        {
            switchObject.position = secondPosition.position;
            Debug.Log("yup");
        }
        else if (switchObject.position == secondPosition.position)
            switchObject.position = thirdPosition.position;
        else
            switchObject.position = startPosition.position;
    }
}
