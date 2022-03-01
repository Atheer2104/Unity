using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;

    public GameObject disc3;
    public float offset, spawnRange;
    public Vector3 spawnBoundaries;
    public float spawnDelay;
    public GameObject enemyParent;
    void OnEnable()
    {
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        while (gameObject.activeSelf == true)
        {
            Instantiate(enemy, new Vector3(Random.Range(-100, 101), 
            Random.Range(-100, 101), 
            Random.Range(player.transform.position.z + offset, player.transform.position.z + offset + spawnRange)), 
            Quaternion.identity, enemyParent.transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
