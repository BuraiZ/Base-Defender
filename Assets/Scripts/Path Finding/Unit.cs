using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public bool displayGizmos;
    public bool isMovementEnabled;
    public bool ignoreObstacles;

    private Target target;
    private Grid grid;
    private Vector3[] path;
    private int targetIndex;
    private float pathUpdateFrequency;
    private float lastpathUpdate;

    private void Awake() {
        grid = GameObject.FindObjectOfType<Grid>();
        pathUpdateFrequency = 0.25f;
        isMovementEnabled = true;
    }

    public void OnDrawGizmos() {
        if (displayGizmos) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(grid.NodeFromWorldPoint(transform.position).worldPosition, Vector3.one);
            if (path != null) {
                for (int i = targetIndex; i < path.Length; i++) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex) {
                        Gizmos.DrawLine(transform.position, path[i]);
                    } else {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }

    public void RequestPath() {
        if (target && target.enabled && isMovementEnabled) {
            if (Time.time - lastpathUpdate >= pathUpdateFrequency) {
                PathRequestManager.RequestPath(transform.position, target.gameObject.transform.position, ignoreObstacles, OnPathFound);
                lastpathUpdate = Time.time;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            targetIndex = 0;
            StopFollowPath();
            StartCoroutine("FollowPath");
        }
    }

    public void StopFollowPath() {
        StopCoroutine("FollowPath");
    }

    private IEnumerator FollowPath() {
        if (path.Length <= 0 || !isMovementEnabled) yield break;

        Vector3 currentWaypoint = path[0];
        while (true) {
            //if (grid.NodeFromWorldPoint(transform.position) == grid.NodeFromWorldPoint(currentWaypoint)) {
            if (Vector3.SqrMagnitude(transform.position - currentWaypoint) < 0.001) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            
            Vector2 direction = (currentWaypoint - transform.position).normalized;
            Vector2 velocity = direction * GetComponent<Monster>().movementSpeed;
            GetComponent<Rigidbody2D>().velocity = velocity;
            yield return null;
        }
    }

    public void SetTarget(Target _target) {
        target = _target;
        target.SubscribeTargetMovement(GetInstanceID(), RequestPath);
        RequestPath();
    }

    public void EnableMovement() {
        isMovementEnabled = true;
    }
}