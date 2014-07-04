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
        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeInput();
    }

    void OnGUI()
    {
        ShowGameModeView();
    }

    void ShowGameModeView()
    {
        if (GUI.Button(new Rect(Screen.width -150, 0, 150, 30), keyButtonText + " (Press Esc)"))
            ChangeInput();
    }

    void ChangeInput()
    {
        keyButtonText = keyButtonText == "Keys" ? "Turk" : "Keys";
        keys = !keys;
        controllerObject.GetComponent<WheelchairController>().Keys = keys;
    }
}
