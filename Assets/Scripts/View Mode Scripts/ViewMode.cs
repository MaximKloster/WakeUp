using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewMode : MonoBehaviour
{
    // Game variables
    [SerializeField]
    GameObject wheelchair, cameraFollower, guiDistance, spriteObject;

    [SerializeField]
    Sprite[] guiDisctanceTextures = new Sprite[6];
    [SerializeField]
    Sprite[] turkElementSprites;

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
    List<TurkSegments> turkElementList = new List<TurkSegments>();
    List<GameObject> turkElementObjectList = new List<GameObject>();

    // GUI variables
    public GameObject ControllerObject { get { return controllerObject; } }
    public float XMedian { get { return xMedian; } }
    public float YMedian { get { return yMedian; } }
    public GUISkin ValueSkin { get { return valueSkin; } }

    public string info;

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

            // Turk segments
            networkView.RPC("SetTurkElements", RPCMode.Others, 0, "start", 0f, "");
            foreach (var turkElement in controllerObject.GetComponent<WheelchairController>().TurkSegmentList)
                networkView.RPC("SetTurkElements", RPCMode.Others, turkElement.segment, turkElement.name, turkElement.distance, turkElement.type);
            networkView.RPC("SetTurkElements", RPCMode.Others, 0, "ready", 0f, "");
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
            && distanceSegments[index] != (int)(distance * 20)) // kann ggf. zur Performanceverbesserung vor dem RPC Call abgefragt werde!!!
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
    [RPC]
    public void SetTurkElements(int segment, string name, float distance, string type)
    {
        if (currentViewMode == ViewModes.TurkMode)
        {
            if (name == "start")
                turkElementList.Clear();
            else if (name == "ready")
                UpdateGUITurkElements();
            else
                turkElementList.Add(new TurkSegments(segment, name, distance, type));
        }
    }
    void UpdateGUITurkElements()
    {
        foreach (var turkElementObject in turkElementObjectList)
            Destroy(turkElementObject);
        turkElementObjectList.Clear();

        foreach (var turkElement in turkElementList)
        {
            Debug.Log(turkElement.segment);

            if (turkElement.segment >= 6 && turkElement.segment <= 7)
            {
                int beta = 90 - (turkElement.segment % 3) * 30;

                float alpha = 90 - beta;

                float x = Mathf.Sin(alpha) * turkElement.distance;
                float y = Mathf.Cos(alpha) * turkElement.distance;

                turkElementObjectList.Add(Instantiate(spriteObject, new Vector3(-x, y, 1.5f), Quaternion.Euler(0, 0, 0)) as GameObject);
                turkElementObjectList[turkElementObjectList.Count - 1].GetComponent<SpriteRenderer>().sprite =
                    turkElement.type == "Curtain" ? turkElementSprites[0] : turkElementSprites[0];
            }
        }

        
    }
}
