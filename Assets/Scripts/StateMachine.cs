using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    private State current;
    public State Current { get; set; }
    Dictionary<Type, State> stateDictionary = new Dictionary<Type, State>();
    
    public void TransitionState<T>() where T : State, new() {
        if (!stateDictionary.ContainsKey(typeof(T)))
            stateDictionary.Add(typeof(T), new T().Init(this, GetComponent<Character>()));

        if (current != null) current.OnExitState();
        current = stateDictionary[typeof(T)];
        current.OnEnterState();
    }

    void Update() {
        if (current != null) current.OnUpdate();
    }
}
