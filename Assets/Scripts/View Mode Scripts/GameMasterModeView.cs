using UnityEngine;
using System.Collections;

public class GameMasterModeView : MonoBehaviour
{
    // variables
    GUISkin valueSkin;

    // Use this for initialization
    void Start()
    {
        valueSkin = transform.GetComponent<ViewMode>().ValueSkin;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        ShowGameMasterModeView();
    }

    void ShowGameMasterModeView()
    {
        for(int i = 0; i < transform.GetComponent<ViewMode>().MasterElementList.Count; i++)
        {
            if (GUI.Button(new Rect(Screen.width / 15, Screen.height / 10 * i, 200, Screen.height / 10), transform.GetComponent<ViewMode>().MasterElementList[i].type))
                GUI.Label(new Rect(Screen.width / 15 + 200, Screen.height / 10 * i, 200, Screen.height / 10), "klicked");
        }

        //GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "You are game master!", valueSkin.label);

        //GUI.Label(new Rect(10, Screen.height - 100, 200, 50), "Info: " + transform.GetComponent<ViewMode>().info, valueSkin.label);
    }
}
