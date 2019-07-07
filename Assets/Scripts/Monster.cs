using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Monster : Character {
    //stat and value
    public int damage;
    public int cashValue;
    public int xpValue;
    [HideInInspector] public float attackRange;

    //components
    private Unit unit;
    protected Target target;

    //gizmo
    public bool displayRaycast;

    //properties
    protected RaycastHit2D[] hits;
    protected GameObject strikeBox;


    //getters & setters
    public Unit Unit { get { return unit; } }
    public Target Target { get { return target; } }


    protected override void Awake() {
        base.Awake();
        unit = GetComponent<Unit>();
    }

    protected override void Start() {
        base.Start();

        //client has no need for logic components
        if (!isServer) {
            unit.enabled = false;
            GetComponent<NetworkTransform>().enabled = false;
        }

        rotationSmooth = 10f;
        stateMachine.TransitionState<SearchTarget>();   //initial state
    }

    protected override void Update() {
        base.Update();

        //if target dies, search new target
        if (!target || !target.enabled) {
            stateMachine.TransitionState<SearchTarget>();
        }
    }

    void OnDrawGizmos() {
        if (displayRaycast && target) {
            Gizmos.color = Color.red;
            Vector2 direction = (target.gameObject.transform.position - transform.position).normalized * attackRange;
            Gizmos.DrawRay(transform.position, direction);
        }
    }

    public virtual void OnSearchTargetEnter() { }
    public virtual void OnSearchTargetExit() { }
    public virtual void OnSearchTarget() {
        if (target) {
            stateMachine.TransitionState<Chase>();
            return;
        }
        Target[] candidates = SearchForTarget();
        SelectRandomTarget(candidates);
    }

    public virtual void OnChaseEnter() {
        unit.isMovementEnabled = true;
        unit.RequestPath();
    }
    public virtual void OnChaseExit() { }
    public virtual void OnChase() {
        SearchTarget();
    }

    public virtual void OnAttackEnter() {
        StopMovement();
        RpcPlayAttackAnimation();
    }
    public virtual void OnAttackExit() { }
    public virtual void OnAttack() {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attacking")) {
            stateMachine.TransitionState<Chase>();
        }
    }

    [Server]
    public override void OnDyingEnter() {
        StopMovement();
        if (target) target.GetComponent<Target>().UnsubscribeTargetMovement(unit.GetInstanceID());
        GetComponent<Unit>().enabled = false;

        RpcOnDying();
    }
    public override void OnDyingExit() { }
    public override void OnDying() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dead")) {
            stateMachine.TransitionState<Dead>();
        }
    }

    public override void OnDeadEnter() {
        Invoke("DestroyObject", deathTimer);
    }
    public override void OnDeadExit() { }
    public override void OnDead() { }

    [Server]
    protected virtual Target[] SearchForTarget() {
        List<Target> targets = new List<Target>();
        foreach (PlayerController player in GameObject.FindObjectsOfType<PlayerController>()) {
            if (player.enabled) targets.Add(player.GetComponent<Target>());
        }

        if (targets.Count == 0) {
            foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower")) {
                if (tower.activeSelf) targets.Add(tower.GetComponent<Target>());
            }

            if (targets.Count == 0) {
                GameObject core = GameObject.Find("Base");
                if (core) targets.Add(core.GetComponent<Target>());
            }
        }

        return targets.ToArray();
    }

    [ClientRpc]
    protected virtual void RpcPlayAttackAnimation() {
    }

    [Server]
    protected void SelectRandomTarget(Target[] targets) {
        if (targets.Length == 0) return;

        target = targets[Random.Range(0, targets.Length)];
        unit.SetTarget(target);
    }

    [Server]
    public void SearchTarget() {
        Vector2 direction = (target.gameObject.transform.position - transform.position).normalized;
        hits = Physics2D.RaycastAll(transform.position, direction, attackRange);

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.gameObject.GetInstanceID() == target.gameObject.GetInstanceID()) {
                stateMachine.TransitionState<Attack>();
                AttackSpriteOrientation();
            }
        }
    }

    /*
     * Ajust the orientation of attacking sprites
     */
    protected void AttackSpriteOrientation() {
        Vector2 direction = (target.gameObject.transform.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + spriteAngleOffset;

        Quaternion tiltAroundZ = Quaternion.Euler(0, 0, angle);
        GetComponent<Transform>().rotation = Quaternion.Slerp(
            GetComponent<Transform>().rotation, tiltAroundZ, 1);
    }

    /*
     * Make object immobile and disable following target
     */
    [Server]
    protected void StopMovement() {
        unit.StopFollowPath();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        unit.isMovementEnabled = false;
    }

    [ClientRpc]
    private void RpcOnDying() {
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("isDead", true);
    }

    [Server]
    public void DealDamageToTarget() {
        if (strikeBox.GetComponent<Collider2D>().bounds.Contains(target.transform.position))
            target.GetComponent<Health>().TakeDamage("monster", damage);
    }

    [Server]
    private void DestroyObject() {
        NetworkServer.Destroy(gameObject);
    }
}
