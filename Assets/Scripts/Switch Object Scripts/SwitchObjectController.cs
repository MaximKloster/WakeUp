using UnityEngine;
using System.Collections;

public class SwitchObjectController : MonoBehaviour
{
    // variables
    Transform firstPosition, secondPosition, switchObject;

    // Use this for initialization
    void Start()
    {
        firstPosition = transform.FindChild("First Position").transform;
        secondPosition = transform.FindChild("Second Position").transform;
        switchObject = transform.FindChild("Switch Object").transform;

        switchObject.position = firstPosition.position;
    }

    public void SwitchPosition()
    {
        if (switchObject.position == firstPosition.position)
            switchObject.position = secondPosition.position;
        else
            switchObject.position = firstPosition.position;
    }
}
