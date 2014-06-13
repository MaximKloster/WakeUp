using UnityEngine;
using System.Collections;

public class ViewMode : MonoBehaviour
{
    // Game variables
    [SerializeField]
    GameObject spawnPoint, wheelchair, cameraFollower, guiDistance;
    [SerializeField]
    Sprite[] guiDisctanceTextures = new Sprite[6];

    [SerializeField]
    GUISkin ValueSkin;

    // Mode/Network variables
    enum ViewModes { GameMode, GameMasterMode, TurkMode }
    ViewModes currentViewMode = ViewModes.GameMode;
    bool chooseViewMode = true, viewModeSetted, tryToConnect = true;
    GameObject controllerObject, guiDistanceObject;
    NetworkConnection networkConnection;

    // GUI variables
    bool keys;
    string keyButtonText = "Keys";
    float xMedian, yMedian;
    int[] distanceSegments = new int[12];
    int collisionDistance = 10;

    // Use this for initialization
    void Start()
    {
        networkConnection = GetComponent<NetworkConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (viewModeSetted)
        {
            if (currentViewMode == ViewModes.GameMode)
            {
                SetControllerObject(wheelchair);
                networkConnection.InitializeServer("WheelchairProject");
            }
            else
            {
                SetControllerObject(cameraFollower);
                controllerObject.GetComponentInChildren<Camera>().camera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                networkConnection.RefreshHostListStandalone = true;

                if (currentViewMode == ViewModes.TurkMode)
                {
                    guiDistanceObject = Instantiate(guiDistance) as GameObject;
                }
            }

            viewModeSetted = false;
        }

        #region Connection
        // Try to connect
        if (currentViewMode != ViewModes.GameMode && tryToConnect && !networkConnection.Connected
            && networkConnection.GetHostDataList().Count > 0)
            foreach (HostData hostData in networkConnection.GetHostDataList())
                if (hostData.gameName == "WheelchairProject")
                {
                    Debug.Log("try connect " + hostData.gameName);
                    networkConnection.ConnectToServer(hostData);
                    networkConnection.RefreshHostListStandalone = false;
                    tryToConnect = false;
                }
        // Send values to clients
        if (currentViewMode == ViewModes.GameMode && networkConnection.Connected)
        {
            // Input
            networkView.RPC("SetControllerValues", RPCMode.Others,
                controllerObject.transform.position,
                controllerObject.transform.rotation,
                controllerObject.GetComponent<WheelchairController>().XMedian,
                controllerObject.GetComponent<WheelchairController>().YMedian);

            // Collision distance
            for (int i = 0; i < controllerObject.GetComponent<WheelchairController>().DistanceSegments.Length; i++)
                networkView.RPC("SetCollisionDistance", RPCMode.Others, i, controllerObject.GetComponent<WheelchairController>().DistanceSegments[i]);
        }
        #endregion
    }

    void SetControllerObject(GameObject controller)
    {
        controllerObject = Instantiate(controller, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height - 50, 300, 50), "Net: " + networkConnection.Connected, ValueSkin.label);

        if (chooseViewMode)
        {
            if (GUI.Button(new Rect(100, 10, 100, 50), "GameMode", ValueSkin.button))
                SetViewMode(ViewModes.GameMode);
            if (GUI.Button(new Rect(250, 10, 100, 50), "MasterMode", ValueSkin.button))
                SetViewMode(ViewModes.GameMasterMode);
            if (GUI.Button(new Rect(400, 10, 100, 50), "TurkMode", ValueSkin.button))
                SetViewMode(ViewModes.TurkMode);
        }

        if (currentViewMode == ViewModes.GameMode && networkConnection.Connected)
            ShowGameModeView();
        else if (currentViewMode == ViewModes.GameMasterMode && networkConnection.Connected)
            ShowGameMasterModeView();
        else if (currentViewMode == ViewModes.TurkMode && networkConnection.Connected)
            ShowTurkModeView();
    }

    void SetViewMode(ViewModes viewMode)
    {
        currentViewMode = viewMode;
        chooseViewMode = false;
        viewModeSetted = true;
    }

    void ShowGameModeView()
    {
        if (GUI.Button(new Rect(Screen.width / 15, Screen.height / 15, 100, 20), keyButtonText))
        {
            keyButtonText = keyButtonText == "Keys" ? "Turk" : "Keys";
            keys = !keys;
            controllerObject.GetComponent<WheelchairController>().Keys = keys;
        }

        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "X Med: " + Mathf.Round(controllerObject.GetComponent<WheelchairController>().XMedian), ValueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 50, 200, 50), "Y Med: " + Mathf.Round(controllerObject.GetComponent<WheelchairController>().XMedian), ValueSkin.label);
    }
    void ShowGameMasterModeView()
    {
        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "You are game master!", ValueSkin.label);
    }
    void ShowTurkModeView()
    {
        GUI.Label(new Rect(Screen.width / 3, Screen.height - 50, 200, 50), "X Med: " + Mathf.Round(xMedian), ValueSkin.label);
        GUI.Label(new Rect(Screen.width / 2, Screen.height - 50, 200, 50), "Y Med: " + Mathf.Round(yMedian), ValueSkin.label);
    }
    void UpdateGUIDistanceTexture(int segment)
    {
        float distance = distanceSegments[segment];
        guiDistanceObject.transform.FindChild(segment.ToString()).GetComponent<SpriteRenderer>().sprite =
            distance >= collisionDistance || distance == -1 ? guiDisctanceTextures[0] :
            distance >= collisionDistance * 0.7 ? guiDisctanceTextures[1] :
            distance >= collisionDistance * 0.4 ? guiDisctanceTextures[2] :
            distance >= collisionDistance * 0.2 ? guiDisctanceTextures[3] :
            distance >= collisionDistance * 0.1 ? guiDisctanceTextures[4] :
            guiDisctanceTextures[5];
    }

    // RPCs
    [RPC]
    public void SetControllerValues(Vector3 controllerPosition, Quaternion controllerRotation, float xMedian, float yMedian)
    {
        // Move camera follower like oculus camera
        controllerObject.transform.position = controllerPosition;
        controllerObject.transform.rotation = controllerRotation;
        // Set values
        this.xMedian = xMedian;
        this.yMedian = yMedian;
    }
    [RPC]
    public void SetCollisionDistance(int index, float distance)
    {
        if (currentViewMode == ViewModes.TurkMode
            && distanceSegments[index] != (int)distance)
        {
            distanceSegments[index] = (int)distance;
            UpdateGUIDistanceTexture(index);
        }
    }
}
