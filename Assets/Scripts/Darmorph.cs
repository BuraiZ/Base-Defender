using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Darmorph : Monster {
    private struct AttackPattern {
        public readonly float range;
        public readonly string animation;
        public readonly GameObject attackCollider;
        public AttackPattern(float _range, string _animation, GameObject _attackCollider) {
            range = _range;
            animation = _animation;
            attackCollider = _attackCollider;
        }

    }
    private List<AttackPattern> attackPatterns;
    private int nextAttackIndex;
    public GameObject bladeCollider;
    public GameObject axeCollider;
    public GameObject deathParticlePrefab;

    protected override void Awake() {
        base.Awake();

        attackPatterns = new List<AttackPattern>();
        attackPatterns.Add(new AttackPattern(4f, "blade attack", bladeCollider));
        attackPatterns.Add(new AttackPattern(2.5f, "axe attack", axeCollider));
    }

    protected override void Start() {
        base.Start();

        PrepareNextAttack();
        attackRange = attackPatterns[nextAttackIndex].range;
    }

    private void PrepareNextAttack() {
        nextAttackIndex = Random.Range(0, attackPatterns.Count);
        attackRange = attackPatterns[nextAttackIndex].range;
        strikeBox = attackPatterns[nextAttackIndex].attackCollider;
        animator.SetInteger("attack index", nextAttackIndex);
    }

    public override void OnAttackEnter() {
        if (Vector3.Distance(transform.position, target.transform.position) < attackRange - 1) StopMovement();
        RpcPlayAttackAnimation();
    }
    public override void OnAttack() {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("blade attack") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("axe attack")) {
            stateMachine.TransitionState<Chase>();
        } else {
            if (Vector3.Distance(transform.position, target.transform.position) > attackRange-1) stateMachine.TransitionState<Chase>();
            else StopMovement();
        }
    }

    public override void OnDeadEnter() {
        base.OnDeadEnter();
        RpcDeathExplosion();
    }

    [ClientRpc]
    private void RpcDeathExplosion() {
        GameObject deathParticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation) as GameObject;
        Destroy(deathParticle, 3);
    }

    [ClientRpc]
    protected override void RpcPlayAttackAnimation() {
        //because the host is both the server and client, this function executes 2 times. The if condition fix the bug of
        //attacking twice per trigger
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("blade attack") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("axe attack")) animator.SetTrigger("attack trigger");
    }

    [Server]
    protected override Target[] SearchForTarget() {
        List<Target> targets = new List<Target>();
        foreach (PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
            if (player.enabled) targets.Add(player.GetComponent<Target>());
        }

        return targets.ToArray();
    }
}
