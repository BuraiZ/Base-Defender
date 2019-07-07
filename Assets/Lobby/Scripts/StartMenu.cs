using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;

public class StartMenu : MonoBehaviour {
    public LobbyManager lobbyManager;

    public void OnClickStartGame() {
        lobbyManager.OpenLobby();
    }
}
