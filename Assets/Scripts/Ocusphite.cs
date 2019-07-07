using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ocusphite : Monster {
    public GameObject attackCollider;

    protected override void Start() {
        base.Start();

        attackRange = 1.5f;
        strikeBox = attackCollider;
    }

    [ClientRpc]
    protected override void RpcPlayAttackAnimation() {
        //because the host is both the server and client, this function executes 2 times. The if condition fix the bug of
        //attacking twice per trigger
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attacking")) animator.SetTrigger("attack trigger");
    }

    [Server]
    protected override Target[] SearchForTarget() {
        List<Target> targets = new List<Target>();
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower")) {
            if (tower.activeSelf) targets.Add(tower.GetComponent<Target>());
        }

        if (targets.Count == 0) {
            GameObject core = GameObject.Find("Base");
            if (core) targets.Add(core.GetComponent<Target>());
        }

        return targets.ToArray();
    }
}
