using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class PlayerController : Character {
    public GameObject playerPrefab;

    private Shooter shooter;
    private Vector2 spawnPosition;
    private bool isInputEnabled;

    protected override void Awake() {
        base.Awake();

        shooter = GetComponent<Shooter>();
    }

    protected override void Start () {
        base.Start();

        rotationSmooth = 15f;

        spawnPosition = new Vector2(0, 5);
        transform.position = spawnPosition;
        isInputEnabled = true;

        stateMachine.TransitionState<Manual>();

        if (isLocalPlayer) {
            GameObject.FindObjectOfType<PlayerCamera>().SyncCameraToPlayer(gameObject);
            GameObject.FindObjectOfType<HealthBar>().SyncObjectToHealthBar(GetComponent<Health>());
        }
    }

    public virtual void OnManualEnter() { }
    public virtual void OnManualExit() { }
    public virtual void OnManual() {
        if (isLocalPlayer) HandleKeyInput();
    }

    public override void OnDyingEnter() {
        RpcOnDeath();
        if (isServer) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Target>().enabled = false;
        }
    }
    public override void OnDyingExit() { }
    public override void OnDying() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("dead")) {
            stateMachine.TransitionState<Dead>();
        }
    }

    public override void OnDeadEnter() {
        if (isServer) {
            StartCoroutine("RespawnPlayer");
            GetComponent<PlayerController>().enabled = false;
        }
    }
    public override void OnDeadExit() { }
    public override void OnDead() { }

    [ClientRpc]
    private void RpcOnDeath() {
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("isDead", true);
        if (isLocalPlayer) {
            GameObject.FindObjectOfType<PlayerCamera>().EnableCamKeyInput(true);
            isInputEnabled = false;
        }
    }

    private IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(deathTimer);

        RpcRespawnOnClient();

        if (isServer) {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Target>().enabled = true;
            GetComponent<Health>().FullHealth();
        }

        stateMachine.TransitionState<Manual>();
    }

    [ClientRpc]
    private void RpcRespawnOnClient() {
        animator.SetBool("isDead", false);
        if (isLocalPlayer) {
            GameObject.FindObjectOfType<PlayerCamera>().EnableCamKeyInput(false);
            isInputEnabled = true;
        }
        transform.position = spawnPosition;
    }

    private void HandleKeyInput() {
        if (isInputEnabled) {
            HandleMovementInput();
            HandleShootingInput();
        }
    }

    private void HandleMovementInput() {
        float xVelocity = 0, yVelocity = 0;

        if (Input.GetKey("down")) {
            yVelocity = -movementSpeed;
        }

        if (Input.GetKey("up")) {
            yVelocity = movementSpeed;
        }

        if (Input.GetKey("left")) {
            xVelocity = -movementSpeed;
        }

        if (Input.GetKey("right")) {
            xVelocity = movementSpeed;
        }

        if (xVelocity == 0 || yVelocity == 0) {
            animator.SetBool("isWalking", false);
        } else {
            animator.SetBool("isWalking", true);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity);
    }

    private void HandleShootingInput() {
        if (Input.GetKey("z")) {
            CmdShoot();
        }

        if (Input.GetKey("x")) {
            //some special power...
        }
    }

    [Command]
    private void CmdShoot() {
        shooter.BeginFire(-transform.up, GetComponent<PlayerInfo>().GetName());
    }
}
