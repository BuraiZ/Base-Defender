using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public AudioClip fireSound;
    public GameObject hitParticlePrefab;
    public float speed;
    public int damage;
    public float fireRate;
    public float chargeDuration;
    protected string shooterName;
    private float spriteAngleOffset;

    protected virtual void Start() {
        spriteAngleOffset = 90;                 //monster facing south on init
    }

    public void SetInitialVelocity(Vector2 direction) {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public void SetShooterName(string name) {
        shooterName = name;
    } 

    public virtual void OnHit(Collider2D collider) {
        Health objectHealth = collider.gameObject.GetComponent<Health>();
        if (objectHealth) {
            objectHealth.TakeDamage(shooterName, damage);
        }

        GameObject hitSplash = Instantiate(hitParticlePrefab, transform.position, transform.rotation) as GameObject;
        Destroy(hitSplash, 3);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag != "Zone") OnHit(collider);
    }
}
