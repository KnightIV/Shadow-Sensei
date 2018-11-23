using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class FileTransferNetworkManager : NetworkManager {

    public const short TECHNIQUE_SENT = 5309;

    public UnityEvent OnTechniqueReceived, OnTechniqueSent;

    private List<NetworkConnection> connections;

    void Start() {
        connections = new List<NetworkConnection>();
    }

    public void SendTechnique(string techniqueName) {
        Technique t = TechniqueFileHelper.LoadClean(techniqueName);

        string techniqueJson = JsonUtility.ToJson(t);

        NetworkWriter writer = new NetworkWriter();
        writer.Write(techniqueJson);

        StringMessage m = new StringMessage();
        m.Serialize(writer);

        foreach (NetworkConnection conn in connections) {
            conn.Send(TECHNIQUE_SENT, m);
        }

        OnTechniqueSent?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);

        connections.Add(conn);
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);

        conn.RegisterHandler(TECHNIQUE_SENT, ReceiveTechnique);
    }

    private void ReceiveTechnique(NetworkMessage message) {
        StringMessage techniqueJson = message.ReadMessage<StringMessage>();

        Technique t = JsonUtility.FromJson<Technique>(techniqueJson.value);
        TechniqueFileHelper.Save(t);

        OnTechniqueReceived?.Invoke();
    }
}
