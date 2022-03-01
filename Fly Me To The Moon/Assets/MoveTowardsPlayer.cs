using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    public float speed;
    public float existTime;
    public GameObject player;
    public GameObject particle;

    public float health;
    public bool isEnemy;
    private void Start()
    {
        player = FindObjectOfType<ShipController>().gameObject;
        Destroy(gameObject, existTime);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (health <= 0)
        {
            if (isEnemy)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBullet>() != null)
        {
            health -= collision.gameObject.GetComponent<PlayerBullet>().bulletDamage;
        }
    }
}
