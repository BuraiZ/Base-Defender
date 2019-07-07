using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour {
    public float movementSpeed;         //unit per second
    public float deathTimer;

    protected float spriteAngleOffset;
    protected float rotationSmooth;

    [HideInInspector] public Animator animator;
    [HideInInspector] public StateMachine stateMachine;

    protected virtual void Awake() {
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
    }
    
    protected virtual void Start () {
        spriteAngleOffset = 90;                 //monster facing south on init
    }

    protected virtual void Update() {
        HandleSpriteOrientation();
    }

    protected void HandleSpriteOrientation() {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        if (!velocity.Equals(Vector2.zero)) {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + spriteAngleOffset;

            Quaternion tiltAroundZ = Quaternion.Euler(0, 0, angle);
            GetComponent<Transform>().rotation = Quaternion.Slerp(
                GetComponent<Transform>().rotation, tiltAroundZ, Time.deltaTime * rotationSmooth);
        }
    }

    public virtual void OnDyingEnter() { }
    public virtual void OnDyingExit() { }
    public virtual void OnDying() { }

    public virtual void OnDeadEnter() { }
    public virtual void OnDeadExit() { }
    public virtual void OnDead() { }
}
