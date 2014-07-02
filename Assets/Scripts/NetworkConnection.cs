using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public string Name { get; private set; }
    public int Number { get; private set; }
    public Color Color { get; private set; }
    public bool ReadyToStart { get; set; }

    public Player(string name, int number, Color color)
    {
        this.Name = name;
        this.Number = number;
        this.Color = color;
        this.ReadyToStart = false;
    }
}

public class NetworkConnection : MonoBehaviour
{
    // public variables
    public int PortNumber { get; private set; }
    public string GameTypeName { get { return "Game42"; } } // TODO set name
    public List<HostData> HostDataList { get; private set; }
    public bool Connected { get; private set; }
    public List<Player> PlayersGameList { get; set; }
    public int ownPlayerNumber { get; set; }
    public bool RefreshHostListStandalone { get; set; }

    void Start()
    {
        PortNumber = 8611; // TODO set port number
        HostDataList = new List<HostData>();
        PlayersGameList = new List<Player>();
    }

    void Update()
    {
        if (RefreshHostListStandalone)
            RefreshHostList();
    }

    #region Network events
    void OnConnectedToServer()
    {
        Connected = true;
    }
    void OnFailedToConnect()
    {

    }
    void OnServerInitialized()
    {
        Connected = true;
    }
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.RegistrationSucceeded)
            return;
    }
    void OnDisconnectedFromServer()
    {
        Connected = false;
    }
    #endregion

    #region Initialize server
    public void InitializeServer(string name, string comment = "", string password = "", int connections = 2)
    {
        if (!Network.isServer && !Network.isClient)
        {
            if (!string.IsNullOrEmpty(password))
                Network.incomingPassword = password;
            Network.InitializeServer(connections - 1, PortNumber, true);
            MasterServer.RegisterHost(GameTypeName, name, comment);
        }
    }
    #endregion

    #region Close server
    public void CloseServer()
    {
        if (Network.isServer && Network.connections.Length != 0)
            for (int i = 0; i < Network.connections.Length; i++)
                Network.CloseConnection(Network.connections[i], true);

        Network.Disconnect();
    }
    #endregion

    #region Connect to server
    public void ConnectToServer(HostData hostData, string password = "")
    {
        if (hostData != null)
            Network.Connect(hostData, password);
    }
    #endregion

    #region Disconnect from server
    public void DisconnectFromServer()
    {
        if (Network.isClient && Network.connections.Length == 1)
            Network.CloseConnection(Network.connections[0], true);
    }
    #endregion

    #region Host list
    public List<HostData> GetHostDataList()
    {
        return HostDataList;
    }
    public string GetHostDataListAsString()
    {
        string serverList = "";
        foreach (HostData hostData in HostDataList)
            serverList += GetHostDataAsString(hostData) + "\n";
        return serverList;
    }
    public List<string> GetHostDataListAsList()
    {
        List<string> serverList = new List<string>();
        foreach (HostData hostData in HostDataList)
            serverList.Add(GetHostDataAsString(hostData));
        return serverList;
    }
    public string GetHostDataAsString(HostData hostData)
    {
        int i = CheckHostDataInList(hostData);
        if (i >= 0)
            return HostDataList[i].gameName + " with " +
                    HostDataList[i].connectedPlayers + " player(s)\n" + // TODO not player need instead name of host
                    HostDataList[i].comment;
        else
            return "No open server found!\nSearch...";
    }
    int CheckHostDataInList(HostData hostData)
    {
        for (int i = 0; i < HostDataList.Count; i++)
            if (HostDataList[i].Equals(hostData))
                return i;
        return -1;
    }
    // Refresh host list
    public void RefreshHostList()
    {
        MasterServer.RequestHostList(GameTypeName);
        HostDataList.Clear();
        foreach (var hostData in MasterServer.PollHostList())
            HostDataList.Add(hostData);
    }
    #endregion

    /* Manual
     * 
     * // Multiplayer variables
     * NetworkConnection networkConnection;
     * 
     * void Start()
     * {
     *      networkConnection = GetComponent<NetworkConnection>();
     * }
     * 
     * void Update()
     * {
     *      networkConnection.RefreshHostList();
     *      // or on Start()
     *      networkConnection.RefreshHostListStandalone = true;
     * }
     */
}
