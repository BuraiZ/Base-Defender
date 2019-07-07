using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;

public class ScoreBoard : NetworkBehaviour {
    public struct PlayerScore {
        public readonly string playerName;
        public readonly int score;
        public readonly int killCount;
        public readonly int deathCount;
        public readonly int cashEarned;
        public readonly int xpEarned;

        public PlayerScore(string _playerName, int _score, int _killCount, int _deathCount, int _cashEarned, int _xpEarned) {
            playerName = _playerName;
            score = _score;
            killCount = _killCount;
            deathCount = _deathCount;
            cashEarned = _cashEarned;
            xpEarned = _xpEarned;
        }
    }

    public List<PlayerScore> playersScore;
    public string myPlayerName;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        playersScore = new List<PlayerScore>();
        FindLocalPlayer();
    }

    private void FindLocalPlayer() {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players) {
            if (player.isLocalPlayer) myPlayerName = player.GetComponent<PlayerInfo>().GetName();
        }
    }

    [ClientRpc]
    public void RpcGetEndGameResult() {
        PlayerInfo[] playersInfo = GameObject.FindObjectsOfType<PlayerInfo>();

        foreach (PlayerInfo playerInfo in playersInfo) {
            playersScore.Add(new PlayerScore(
                playerInfo.GetName(),
                playerInfo.GetScore(),
                playerInfo.GetKillCount(),
                playerInfo.GetDeathCount(),
                playerInfo.GetCashEarned(),
                playerInfo.GetXPEarned()
            ));
        }
    }

    public PlayerScore GetLocalPlayerInfo() {
        foreach (PlayerScore playerInfo in playersScore) {
            if (playerInfo.playerName == myPlayerName) return playerInfo;
        }
        return new PlayerScore("no name", 0, 0, 0, 0, 0);
    }
}
