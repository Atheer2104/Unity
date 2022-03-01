using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private PlayerShoot player;
    public float bulletSpeed;
    public float bulletDamage;
    private Vector3 target;

    private void Start()
    {
        player = FindObjectOfType<PlayerShoot>();
        target = player.bulletTarget;
        Destroy(gameObject, 7f);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
