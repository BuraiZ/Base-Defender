using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class State {
    protected StateMachine stateMachine;
    protected Character character;

    public virtual State Init(StateMachine _stateMachine, Character _character) {
        stateMachine = _stateMachine;
        character = _character;
        return this;
    }

    public virtual void OnEnterState() { }
    public virtual void OnExitState() { }
    public virtual void OnUpdate() { }
}

public class Dying : State {
    public override void OnEnterState() { character.OnDyingEnter(); }
    public override void OnExitState() { character.OnDyingExit(); }
    public override void OnUpdate() { character.OnDying(); }
}

public class Dead : State {
    public override void OnEnterState() { character.OnDeadEnter(); }
    public override void OnExitState() { character.OnDeadExit(); }
    public override void OnUpdate() { character.OnDead(); }
}

public class Chase : State {
    public override void OnEnterState() { (character as Monster).OnChaseEnter(); }
    public override void OnExitState() { (character as Monster).OnChaseExit(); }
    public override void OnUpdate() { (character as Monster).OnChase(); }
}

public class Attack : State {
    public override void OnEnterState() { (character as Monster).OnAttackEnter(); }
    public override void OnExitState() { (character as Monster).OnAttackExit(); }
    public override void OnUpdate() { (character as Monster).OnAttack(); }
}

public class SearchTarget : State {
    public override void OnEnterState() { (character as Monster).OnSearchTargetEnter(); }
    public override void OnExitState() { (character as Monster).OnSearchTargetExit(); }
    public override void OnUpdate() { (character as Monster).OnSearchTarget(); }
}

public class Manual : State {
    public override void OnEnterState() { (character as PlayerController).OnManualEnter(); }
    public override void OnExitState() { (character as PlayerController).OnManualExit(); }
    public override void OnUpdate() { (character as PlayerController).OnManual(); }
}
