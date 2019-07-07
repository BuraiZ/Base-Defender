using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour {
    private GameObject player;
    private bool enabledKeyShorcut;
    private bool isLockOn;
    private float zDistance;
    private float scrollSpeed;

    private void Start () {
        enabledKeyShorcut = false;
        isLockOn = true;

        zDistance = -10;
        scrollSpeed = 10;
    }

    public void SyncCameraToPlayer(GameObject player) {
        this.player = player;
    }

    private void Update () {
        if (!player) return;

        HandleKeyInput();

        if (isLockOn) {
            transform.position = player.transform.position + Vector3.forward * zDistance;
        } else {
            ControlCamera();
        }
    }

    private void ControlCamera() {
        if (enabledKeyShorcut) {
            if (Input.GetKey("down")) transform.Translate(new Vector3(0, -scrollSpeed * Time.deltaTime, 0));
            if (Input.GetKey("up")) transform.Translate(new Vector3(0, scrollSpeed * Time.deltaTime, 0));
            if (Input.GetKey("left")) transform.Translate(new Vector3(-scrollSpeed * Time.deltaTime, 0, 0));
            if (Input.GetKey("right")) transform.Translate(new Vector3(scrollSpeed * Time.deltaTime, 0, 0));
        }
    }

    private void HandleKeyInput() {
        if (enabledKeyShorcut) {
            if (Input.GetKey("space")) isLockOn = true;
            else if (Input.anyKeyDown) isLockOn = false;
        }
    }

    public void EnableCamKeyInput(bool isEnabled) {
        enabledKeyShorcut = isEnabled;
        if (!isEnabled) isLockOn = true;
    }
}
