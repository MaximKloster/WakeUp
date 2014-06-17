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
        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "You are game master!", valueSkin.label);
    }
}
