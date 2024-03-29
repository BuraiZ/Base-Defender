﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathfinding;

    bool isProcessingPath;

    void Awake() {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
        isProcessingPath = false;
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, bool ignoreObstacles, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext(ignoreObstacles);
    }

    void TryProcessNext(bool ignoreObstacles) {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, ignoreObstacles);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool ignoreObstacles, bool success) {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext(ignoreObstacles);
    }

    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}