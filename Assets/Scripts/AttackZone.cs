using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AttackZone : NetworkBehaviour {
    public Shooter shooter;

    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            Vector2 direction = (collider.gameObject.transform.position - transform.position).normalized;
            CmdFireBullet(direction);
        }
    }

    [Command]
    private void CmdFireBullet(Vector2 direction) {
        shooter.BeginFire(direction, "");
    }
}
