using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class ScoreManager : NetworkBehaviour {
    public GameObject playerScorePrefab;
    public GameObject list;
    public Text goldEarned;
    public Text XPEarned;

    private ScoreBoard scoreBoard;

    private void Start() {
        scoreBoard = GameObject.FindObjectOfType<ScoreBoard>();
        if (!scoreBoard) Debug.LogError("scoreBoard not found!");

        InitializePlayersScore();

        ScoreBoard.PlayerScore playerInfo = scoreBoard.GetLocalPlayerInfo();
        goldEarned.text = playerInfo.cashEarned + "";
        XPEarned.text = playerInfo.xpEarned + "";
    }

    private void InitializePlayersScore() {
        foreach (ScoreBoard.PlayerScore playerScore in scoreBoard.playersScore) {
            GameObject playerInfo = Instantiate(playerScorePrefab);

            Text[] texts = playerInfo.GetComponentsInChildren<Text>();
            foreach (Text text in texts) {
                if (text.name == "Player Name") text.text = playerScore.playerName;
                if (text.name == "Kill") text.text = playerScore.killCount + "";
                if (text.name == "Death") text.text = playerScore.deathCount + "";
            }

            playerInfo.transform.parent = list.transform;
            playerInfo.transform.localScale = new Vector3(1, 1, 1);
        }        
    }

    public void ReturnToLobby() {
        Destroy(scoreBoard.gameObject);
        if (isServer) LobbyManager.s_Singleton.StopHostClbk();
        else LobbyManager.s_Singleton.StopClientClbk();
    }
}
