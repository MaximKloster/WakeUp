using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewMode : MonoBehaviour
{
    #region Variables
    // Game variables
    [SerializeField]
    GameObject wheelchair = null, cameraFollower = null, guiDistance = null, spriteObject = null;

    [SerializeField]
    Sprite[] guiDisctanceTextures = new Sprite[6];
    [SerializeField]
    Sprite[] turkElementSprites;

    [SerializeField]
    GUISkin valueSkin;

    // Mode/Network variables
    enum ViewModes { GameMode, GameMasterMode, TurkMode, ObserverMode }
    ViewModes currentViewMode = ViewModes.GameMode;
    bool chooseViewMode = true, viewModeSetted, tryToConnect = true;
    NetworkConnection networkConnection;
    float xMedian, yMedian;
    GameObject controllerObject;

    // Turk
    GameObject guiDistanceObject;
    int[] distanceSegments = new int[12]; // kann ggf. weg..
    public GameObject SpriteObject { get { return spriteObject; } }
    public GameObject GuiDistanceObject { get { return guiDistanceObject; } }
    public Sprite[] GuiDisctanceTextures { get { return guiDisctanceTextures; } }
    public Sprite[] TurkElementSprites { get { return turkElementSprites; } }

    // GUI variables
    public GameObject ControllerObject { get { return controllerObject; } }
    public float XMedian { get { return xMedian; } }
    public float YMedian { get { return yMedian; } }
    public GUISkin ValueSkin { get { return valueSkin; } }

    public string info; 
    #endregion

    // Use this for initialization
    void Start()
    {
        networkConnection = GetComponent<NetworkConnection>();
    }

    void OnDisconnectedFromServer()
    {
        networkConnection.RefreshHostListStandalone = true;
        tryToConnect = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region Set ViewMode
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
        #endregion

        #region Connection
        // Try to connect
        if (currentViewMode != ViewModes.GameMode && tryToConnect && !networkConnection.Connected
            && networkConnection.GetHostDataList().Count > 0)
        {
            //foreach (HostData hostData in networkConnection.GetHostDataList())
            HostData hostData = networkConnection.GetHostDataList()[0];
            if (hostData.gameName == "WheelchairProject")
            {
                networkConnection.ConnectToServer(hostData);
                networkConnection.RefreshHostListStandalone = false;
                tryToConnect = false;
            }
        }
        // Send values to clients
        if (currentViewMode == ViewModes.GameMode && networkConnection.Connected)
        {
            // Input
            networkView.RPC("SetControllerValues", RPCMode.Others,
                controllerObject.transform.position,
                controllerObject.transform.rotation,
                controllerObject.GetComponent<WheelchairController>().XInput,
                controllerObject.GetComponent<WheelchairController>().YInput);

            // Collision distance
            for (int i = 0; i < controllerObject.GetComponent<WheelchairController>().DistanceSegments.Length; i++)
                networkView.RPC("SetCollisionDistance", RPCMode.Others, i, controllerObject.GetComponent<WheelchairController>().DistanceSegments[i]);

            // Turk segments
            networkView.RPC("SetTurkElements", RPCMode.Others, 0, 0f, "start");
            foreach (var turkElement in controllerObject.GetComponent<WheelchairController>().TurkSegmentList)
                networkView.RPC("SetTurkElements", RPCMode.Others, turkElement.segment, turkElement.distance, turkElement.type);
            networkView.RPC("SetTurkElements", RPCMode.Others, 0, 0f, "ready");

            // Master elements
            networkView.RPC("SetMasterElements", RPCMode.Others, "start", 0);
            foreach (var masterElement in controllerObject.GetComponent<WheelchairController>().MasterElementList)
                networkView.RPC("SetMasterElements", RPCMode.Others, masterElement.type, masterElement.ID);
            networkView.RPC("SetMasterElements", RPCMode.Others, "ready", 0);
        }
        #endregion
    }
    void SetControllerObject(GameObject controller)
    {
        controllerObject = Instantiate(controller, GameObject.FindGameObjectWithTag("Respawn").transform.position, GameObject.FindGameObjectWithTag("Respawn").transform.rotation) as GameObject;
    }

    void OnGUI()
    {
        if(!networkConnection.Connected)
            GUI.Label(new Rect(10, Screen.height - 50, 300, 50), "No connection found!", valueSkin.label);

        if (chooseViewMode)
        {
            if (GUI.Button(new Rect(100, 10, 100, 50), "GameMode", valueSkin.button))
            {
                SetViewMode(ViewModes.GameMode);
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
            if (GUI.Button(new Rect(550, 10, 100, 50), "ObserverMode", valueSkin.button))
                SetViewMode(ViewModes.ObserverMode);
        }
    }
    void SetViewMode(ViewModes viewMode)
    {
        currentViewMode = viewMode;
        chooseViewMode = false;
        viewModeSetted = true;
    }

    // RPCs
    #region Controller
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
    #endregion
    #region Turk Mode
    [RPC]
    public void SetCollisionDistance(int index, float distance)
    {
        if (currentViewMode == ViewModes.TurkMode
            && distanceSegments[index] != (int)(distance * 20)) // kann ggf. zur Performanceverbesserung vor dem RPC Call abgefragt werde!!!
        {
            distanceSegments[index] = (int)(distance * 20);
            GetComponent<TurkModeView>().UpdateGUIDistanceTexture(index, distance);
        }
    }
    [RPC]
    public void SetTurkElements(int segment, float distance, string type)
    {
        if (currentViewMode == ViewModes.TurkMode)
        {
            if (type == "start")
                GetComponent<TurkModeView>().ClearTurkElementList();
            else if (type == "ready")
                GetComponent<TurkModeView>().UpdateGUITurkElements();
            else
                GetComponent<TurkModeView>().AddElementToTurkElementList(new TurkSegment(segment, distance, type));
        }
    } 
    #endregion
    #region Master Mode
    [RPC]
    void SetMasterElements(string type, int ID)
    {
        if (currentViewMode == ViewModes.GameMasterMode)
        {
            if (type == "start")
                GetComponent<GameMasterModeView>().ClearMasterElementList();
            else if (type == "ready")
                return;
            else
                GetComponent<GameMasterModeView>().AddElementToMasterElementList(new MasterElement(type, ID));
        }
    } 
    #endregion
}
