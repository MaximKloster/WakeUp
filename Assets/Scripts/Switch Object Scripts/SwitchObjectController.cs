using UnityEngine;
using System.Collections;

public class SwitchObjectController : MonoBehaviour
{
    // variables
    Transform startPosition, secondPosition, thirdPosition, switchObject;

    // Use this for initialization
    void Start()
    {
        switchObject = transform.FindChild("Switch Object").transform;
        //startPosition = switchObject;
        secondPosition = transform.FindChild("Second Position").transform;
        thirdPosition = transform.FindChild("Third Position").transform;


        startPosition.position = switchObject.position;
    }

    public void SwitchPosition()
    {
        if (switchObject.position == secondPosition.position)
            switchObject.position = thirdPosition.position;
        else
            switchObject.position = secondPosition.position;
        //switchObject.position = secondPosition.position;
    }
}
