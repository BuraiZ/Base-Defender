using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shooter : NetworkBehaviour {
    public GameObject bulletPrefab;
    public GameObject gun;

    private float nextFire;
    private GameObject bulletContainer;
    private AudioSource audioSource;

    private void Awake() {
        bulletContainer = GameObject.Find("Bullet Container");
        if (!bulletContainer) bulletContainer = new GameObject("Bullet Container");
        audioSource = GetComponent<AudioSource>();
    }

    private void Start () {
        nextFire = 0;
    }

    public void BeginFire(Vector2 direction, string shooterName) {
        if (nextFire <= Time.time) {
            if (bulletPrefab.GetComponent<Bullet>().chargeDuration != 0) StartCoroutine(Charge(direction, shooterName));
            else FireBullet(direction, shooterName);
        }
    }

    private IEnumerator Charge(Vector2 direction, string shooterName) {
        yield return new WaitForSeconds(bulletPrefab.GetComponent<Bullet>().chargeDuration);
        FireBullet(direction, shooterName);
    }

    public void FireBullet(Vector2 direction, string shooterName) {
        audioSource.clip = bulletPrefab.GetComponent<Bullet>().fireSound;
        audioSource.Play();

        nextFire = Time.time + bulletPrefab.GetComponent<Bullet>().fireRate;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
        bullet.GetComponent<Bullet>().SetInitialVelocity(direction);
        bullet.GetComponent<Bullet>().SetShooterName(shooterName);

        NetworkServer.Spawn(bullet);

        bullet.transform.parent = bulletContainer.transform;
    }
}
