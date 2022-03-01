using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public Transform rotator;
    public GameObject meteor;
    public GameObject[] meteorSpawnpoint;
    private int r;
    public float spawnDelay;

    private void Start()
    {
        StartCoroutine("MeteorSpawn");
    }
    private void Update()
    {
        r = Random.Range(0, meteorSpawnpoint.Length);
    }
    private IEnumerator MeteorSpawn()
    {
        while (true)
        {
            Instantiate(meteor, meteorSpawnpoint[r].transform.position, Quaternion.identity, rotator);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
