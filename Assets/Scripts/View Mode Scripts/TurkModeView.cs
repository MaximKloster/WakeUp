using UnityEngine;
using System.Collections;

public class TurkModeView : MonoBehaviour
{
    // variables
    ViewMode viewMode;
    GUISkin valueSkin;

    // Use this for initialization
    void Start()
    {
        viewMode = transform.GetComponent<ViewMode>();
        valueSkin = viewMode.ValueSkin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnGUI()
    {
        ShowTurkModeView();
    }

    void ShowTurkModeView()
    {
        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "X: " + Mathf.Round(viewMode.XMedian), valueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 50, 200, 50), "Y: " + Mathf.Round(viewMode.YMedian), valueSkin.label);

        GUI.Label(new Rect(10, Screen.height - 100, 200, 50), "Info: " + viewMode.info, valueSkin.label);
    }
}
