using UnityEngine;
using System.Collections;

public class SwitchObjectController : MonoBehaviour
{
    // variables
    [SerializeField]
    GameObject moveableObject = null;
    [SerializeField]
    bool bla;
    Transform startPosition, secondPosition, thirdPosition, switchObject;

    // Use this for initialization
    void Start()
    {
        //controllerObject = Instantiate(controller, GameObject.FindGameObjectWithTag("Respawn").transform.position, GameObject.FindGameObjectWithTag("Respawn").transform.rotation) as GameObject;

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
