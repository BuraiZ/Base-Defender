using UnityEngine;
using System.Collections;

public class TowerBullet : Bullet {
    public float lifetime;

    public override void OnHit(Collider2D collider) {
        Health objectHealth = collider.gameObject.GetComponent<Health>();
        if (objectHealth) {
            objectHealth.TakeDamage(shooterName, damage);
        }

        Destroy(gameObject, lifetime);
    }
}
