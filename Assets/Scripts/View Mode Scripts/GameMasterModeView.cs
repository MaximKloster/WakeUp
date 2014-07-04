using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMasterModeView : MonoBehaviour
{
    // variables
    GUISkin valueSkin;

    List<MasterElement> masterElementList = new List<MasterElement>();

    // Use this for initialization
    void Start()
    {
        valueSkin = transform.GetComponent<ViewMode>().ValueSkin;
    }

    void OnGUI()
    {
        ShowGameMasterModeView();
    }

    void ShowGameMasterModeView()
    {
        for(int i = 0; i < masterElementList.Count; i++)
        {
            if (GUI.Button(new Rect(Screen.width / 15, Screen.height / 10 * i, 200, Screen.height / 10), masterElementList[i].type))
                GUI.Label(new Rect(Screen.width / 15 + 200, Screen.height / 10 * i, 200, Screen.height / 10), "klicked");
        }

        //GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "You are game master!", valueSkin.label);

        //GUI.Label(new Rect(10, Screen.height - 100, 200, 50), "Info: " + transform.GetComponent<ViewMode>().info, valueSkin.label);
    }

    public void ClearMasterElementList()
    {
        masterElementList.Clear();
    }
    public void AddElementToMasterElementList(MasterElement masterElement)
    {
        masterElementList.Add(masterElement);
    }
}
