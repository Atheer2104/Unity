    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject fireball;
    public GameObject player;
    public Transform mouthPos;
    public float range;
    public float shootDelay;

    private void Start()
    {
        player = FindObjectOfType<ShipController>().gameObject;
        StartCoroutine("Shoot");
    }
    private void FixedUpdate()
    {
        transform.LookAt(player.transform);
    }

    IEnumerator Shoot()
    {
        while (gameObject.activeSelf == true)
        {
            yield return new WaitForSeconds(shootDelay);
            Instantiate(fireball, mouthPos.position, Quaternion.identity, transform.parent);
        }
    }
}
