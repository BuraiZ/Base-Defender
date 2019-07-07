using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public int maxHealthPoint;
    [SyncVar (hook = "OnHealthChange")] public int healthPoint;
    
	private void Awake () {
        healthPoint = maxHealthPoint;
	}

    [Server]
    public void Heal(int health) {
        healthPoint += health;
        if (healthPoint > maxHealthPoint) healthPoint = maxHealthPoint; 
    }

    [Server]
    public void FullHealth() {
        healthPoint = maxHealthPoint;
    }

    [Server]
    public void TakeDamage(string shooter, int damage) {
        healthPoint -= damage;
        if (healthPoint <= 0) {
            healthPoint = 0;
            if (GetComponent<Character>()) {
                DedicateRewardsToShooter(shooter);
                GetComponent<Character>().stateMachine.TransitionState<Dying>();
            } else {
                gameObject.SetActive(false);
            }
        }
    }

    private void DedicateRewardsToShooter(string shooter) {
        if (shooter == "") return;

        if (shooter == "monster") {    //Monster kill player
            GetComponent<PlayerInfo>().AddDeath();
        } else {                //Player kill monster
            Monster monster = GetComponent<Monster>();
            PlayerInfo[] players = GameObject.FindObjectsOfType<PlayerInfo>();
            foreach (PlayerInfo player in players) {
                if (player.GetName() == shooter) {
                    player.AddKill();
                    player.AddCash(monster.cashValue);
                    player.AddXP(monster.xpValue);
                }
            }
        }
    }

    private void OnHealthChange(int health) {
        healthPoint = health;
    }

}
