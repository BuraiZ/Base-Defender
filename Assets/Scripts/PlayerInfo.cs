using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour {
    [SyncVar] private string playerName;
    private int score;
    private int killCount;
    private int deathCount;
    private int cashEarned;
    private int xpEarned;
    
    private void Start() {
        score = 0;
        killCount = 0;
        deathCount = 0;
        cashEarned = 0;
        xpEarned = 0;
    }

    public void SetPlayerName(string name) {
        playerName = name;
    } 

    public void AddScore(int scoreEarned) {
        score += scoreEarned;
    }

    public void AddKill() {
        killCount++;
    }

    public void AddDeath() {
        deathCount++;
    }

    public void AddCash(int newCashEarned) {
        cashEarned += newCashEarned;
    }

    public void AddXP(int newXPEarned) {
        xpEarned += newXPEarned;
    }

    public string GetName() {
        return playerName;
    }

    public int GetScore() {
        return score;
    }

    public int GetKillCount() {
        return killCount;
    }

    public int GetDeathCount() {
        return deathCount;
    }

    public int GetCashEarned() {
        return cashEarned;
    }

    public int GetXPEarned() {
        return xpEarned;
    }
}
