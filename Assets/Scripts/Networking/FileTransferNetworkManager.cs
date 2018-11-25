using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class FileTransferNetworkManager : NetworkManager {

    public const short SENSEI_NAME = 867;
    public const short TECHNIQUE_SENT = 5309;

    public UnityEvent OnTechniqueReceived, OnTechniqueSent;
    public string UserName;

    private List<NetworkConnection> connections;
    private NetworkDiscovery networkDiscovery;

    void Awake() {
        connections = new List<NetworkConnection>();
        networkDiscovery = gameObject.GetComponent<NetworkDiscovery>();
    }

    public void SendTechnique(string techniqueName) {
        Technique t = TechniqueFileHelper.LoadClean(techniqueName);

        string techniqueJson = JsonUtility.ToJson(t);

        foreach (NetworkConnection conn in connections) {
            SendString(TECHNIQUE_SENT, conn, techniqueJson);
        }

        OnTechniqueSent?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn) {
        base.OnServerConnect(conn);

        SendString(SENSEI_NAME, conn, UserName);
        connections.Add(conn);
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);

        conn.RegisterHandler(TECHNIQUE_SENT, ReceiveTechnique);
        conn.RegisterHandler(SENSEI_NAME, ReceivedSenseiName);
    }

    public override void OnStartClient(NetworkClient client) {
        base.OnStartClient(client);

        if (!networkDiscovery.StartAsClient()) {
            Debug.Log("Couldn't start listening");
        }
    }

    public override void OnStartServer() {
        base.OnStartServer();

        if (!networkDiscovery.StartAsServer()) {
            Debug.Log("Couldn't start listening");
        }
    }

    public new void StartHost() {
        base.StartHost();

        networkDiscovery.isServer = true;
    }

    public new void StartClient() {
        base.StartClient();

        networkDiscovery.isClient = true;
    }

    private void ReceiveTechnique(NetworkMessage message) {
        StringMessage techniqueJson = message.ReadMessage<StringMessage>();

        Technique t = JsonUtility.FromJson<Technique>(techniqueJson.value);
        TechniqueFileHelper.Save(t);

        OnTechniqueReceived?.Invoke();
    }

    private void ReceivedSenseiName(NetworkMessage msg) {
        StringMessage nameMsg = msg.ReadMessage<StringMessage>();

        // TODO: do stuff with the name
    }

    private void SendString(short msgType, NetworkConnection conn, string msg) {
        NetworkWriter writer = new NetworkWriter();
        writer.Write(msg);

        StringMessage stringMessage = new StringMessage();
        stringMessage.Serialize(writer);

        conn.Send(msgType, stringMessage);
    }
}
