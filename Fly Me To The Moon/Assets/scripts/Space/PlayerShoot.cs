using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    public Vector3 bulletTarget;
    private PlayerController player;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootTime;
    private float time;

    public AudioSource audioSource;

    public LayerMask mask;
    private void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    if (hit.collider != null)
                    {
                        bulletTarget = hit.point;
                        OnShoot();
                        
                    }
                }
            }
    }
    public void OnShoot()
    {
        if (shootTime < time)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            audioSource.Play(0);
            time = 0;
        }
    }
}
