using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {
    public GameObject[] monsterPrefabs;
    public float minSpawnRate;
    public float maxSpawnRate;
    public float initialSpawnDelay;
    private GameObject monsterContainer;

    private void Awake() {
        monsterContainer = GameObject.Find("Monster Container");
        if (!monsterContainer) monsterContainer = new GameObject("Monster Container");
    }

    private void Start () {
        StartCoroutine("SpawnNextEnemy");
    }
    
    private IEnumerator SpawnNextEnemy() {
        yield return new WaitForSeconds(initialSpawnDelay);
        while (true) {
            SpawnRandomMonster();
            float nextDelay = Random.Range(minSpawnRate, maxSpawnRate);
            yield return new WaitForSeconds(nextDelay);
        }
    }

    private void SpawnRandomMonster() {
        GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        GameObject monsterSpawned = Instantiate(monsterPrefab, transform.position, transform.rotation) as GameObject;
        monsterSpawned.transform.parent = monsterContainer.transform;
        NetworkServer.Spawn(monsterSpawned);
    }
}
