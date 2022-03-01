using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    public Transform meteorSpawnPoints;
    private Vector3 targetPos;
    public float meteorSpeed;
    public float minSize, maxSize;
    public float timeToDestroy;
    public float maxDistance;
    private float r, t;

    private void Start()
    {
        meteorSpawnPoints = FindObjectOfType<PlayerOffsetFollow>().transform;
        r = Random.Range(minSize, maxSize);
        meteorSpeed = meteorSpeed * 1 / (transform.localScale.x * r);
        gameObject.transform.localScale = new Vector3(transform.localScale.x * r, transform.localScale.y * r, transform.localScale.z * r);
        targetPos = meteorSpawnPoints.position + new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), 0);

        Destroy(gameObject, 30f);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, meteorSpeed * Time.deltaTime);

        if (transform.position == targetPos)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }
}
