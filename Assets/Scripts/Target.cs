using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class Target : NetworkBehaviour {
    private Dictionary<int, Action> pathCallbacks;

    private void Awake() {
        pathCallbacks = new Dictionary<int, Action>();
    }

    private void Start() {
        if (!isServer) GetComponent<Target>().enabled = false;
    }

    private void Update() {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        if (velocity.x != 0 || velocity.y != 0) {
            /*if (isServer)*/ RpcUpdateMonsterPath();    //request path update whenever player moves
        }
    }

    public void SubscribeTargetMovement(int id, Action callback) {
        if (!IsKeySubscribed(id)) pathCallbacks.Add(id, callback);
    }

    public void UnsubscribeTargetMovement(int id) {
        pathCallbacks.Remove(id);
    }

    public bool IsKeySubscribed(int id) {
        return pathCallbacks.ContainsKey(id);
    }

    //[ClientRpc]
    private void RpcUpdateMonsterPath() {
        List<int> keys = new List<int>(pathCallbacks.Keys);
        foreach (int key in keys) {
            pathCallbacks[key]();
        }
    }
}
