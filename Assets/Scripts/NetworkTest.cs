using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkTest : NetworkBehaviour {
    
    public override void OnStartClient() {
        if (!isServer) ClientFunction();
        if (!isServer) CmdRunOnServer();
        if (isServer) ServerFunction();
        if (isServer) RpcRunOnClient();
    }

    [Client]
    public void ClientFunction() {
        print("I am the client");
    }

    [Command]
    public void CmdRunOnServer() {
        print("The client called me to run on the server.");
    }

    [Server]
    public void ServerFunction() {
        print("I am the server, I handle the important decisions.");
    }

    [ClientRpc]
    public void RpcRunOnClient() {
        print("The server called me to run on the client");
    }
}