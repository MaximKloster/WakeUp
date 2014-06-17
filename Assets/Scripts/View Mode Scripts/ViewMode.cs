using UnityEngine;
using System.Collections;

public class ViewMode : MonoBehaviour
{
    // Game variables
    [SerializeField]
    GameObject wheelchair, cameraFollower, guiDistance;

    [SerializeField]
    Sprite[] guiDisctanceTextures = new Sprite[6];

    [SerializeField]
    GUISkin valueSkin;

    // Mode/Network variables
    enum ViewModes { GameMode, GameMasterMode, TurkMode }
    ViewModes currentViewMode = ViewModes.GameMode;
    bool chooseViewMode = true, viewModeSetted, tryToConnect = true;
    GameObject controllerObject, guiDistanceObject;
    NetworkConnection networkConnection;
    float xMedian, yMedian;
    int[] distanceSegments = new int[12];
    int collisionDistance = 60;

    // GUI variables
    public GameObject ControllerObject { get { return controllerObject; } }
    public float XMedian { get { return xMedian; } }
    public float YMedian { get { return yMedian; } }
    public GUISkin ValueSkin { get { return valueSkin; } }

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
        controllerObject = Instantiate(controller, GameObject.FindGameObjectWithTag("Respawn").transform.position, GameObject.FindGameObjectWithTag("Respawn").transform.rotation) as GameObject;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, Screen.height - 50, 300, 50), "Net: " + networkConnection.Connected, valueSkin.label);

        if (chooseViewMode)
        {
            if (GUI.Button(new Rect(100, 10, 100, 50), "GameMode", valueSkin.button))
            {
                SetViewMode(ViewModes.GameMode);
                Screen.showCursor = false;
                gameObject.AddComponent("GameModeView");
            }
            if (GUI.Button(new Rect(250, 10, 100, 50), "MasterMode", valueSkin.button))
            {
                SetViewMode(ViewModes.GameMasterMode);
                gameObject.AddComponent("GameMasterModeView");
            }
            if (GUI.Button(new Rect(400, 10, 100, 50), "TurkMode", valueSkin.button))
            {
                SetViewMode(ViewModes.TurkMode);
                gameObject.AddComponent("TurkModeView");
            }
        }
    }

    void SetViewMode(ViewModes viewMode)
    {
        currentViewMode = viewMode;
        chooseViewMode = false;
        viewModeSetted = true;
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
            && distanceSegments[index] != (int)(distance * 20))
        {
            distanceSegments[index] = (int)(distance * 20);
            UpdateGUIDistanceTexture(index);
        }
    }
    void UpdateGUIDistanceTexture(int segment)
    {
        float distance = distanceSegments[segment];
        guiDistanceObject.transform.FindChild(segment.ToString()).GetComponent<SpriteRenderer>().sprite =
            distance >= collisionDistance * 0.9 || distance < 0 ? guiDisctanceTextures[0] :
            distance >= collisionDistance * 0.55 ? guiDisctanceTextures[1] :
            distance >= collisionDistance * 0.35 ? guiDisctanceTextures[2] :
            distance >= collisionDistance * 0.25 ? guiDisctanceTextures[3] :
            distance >= collisionDistance * 0.15 ? guiDisctanceTextures[4] :
            guiDisctanceTextures[6];
    }
}
