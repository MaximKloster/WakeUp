using UnityEngine;
using System.Collections;

public class GameModeView : MonoBehaviour
{
    // variables
    GameObject controllerObject;
    GUISkin valueSkin;

    bool keys;
    string keyButtonText = "Keys";

    // Use this for initialization
    void Start()
    {
        controllerObject = transform.GetComponent<ViewMode>().ControllerObject;
        valueSkin = transform.GetComponent<ViewMode>().ValueSkin;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        ShowGameModeView();
    }

    void ShowGameModeView()
    {
        if (GUI.Button(new Rect(Screen.width / 15, Screen.height / 15, 100, 20), keyButtonText))
        {
            keyButtonText = keyButtonText == "Keys" ? "Turk" : "Keys";
            keys = !keys;
            controllerObject.GetComponent<WheelchairController>().Keys = keys;
        }

        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "X Med: " + Mathf.Round(controllerObject.GetComponent<WheelchairController>().XMedian), valueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 50, 200, 50), "Y Med: " + Mathf.Round(controllerObject.GetComponent<WheelchairController>().YMedian), valueSkin.label);
    }
}
