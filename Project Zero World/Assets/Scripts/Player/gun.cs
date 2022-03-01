using UnityEngine;
using System.Collections;

public class gun : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;

    public int maxAmmo = 10;
    private int currentAmmo;
    // reload time is by default one second 
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera playerCamera;
    public ParticleSystem muzzleflash;

    private float nextTimeToFire = 0f;

    void Start() {
        currentAmmo = maxAmmo;
    }

    void OnEnable() {
        isReloading = false;
    }

    // Update is called once per frame
    void Update() {

        if (isReloading) {
            return;
        }

        if (currentAmmo <= 0) {
            StartCoroutine(reload());
            return;
        }

        // TODO: add photonview.ismine later 
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + (1f/fireRate);
            Debug.Log(nextTimeToFire);
            shoot();
        }   
    }

    IEnumerator reload() {
        isReloading = true;
        Debug.Log("reloading...");

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void shoot() {
        muzzleflash.Play();

        currentAmmo--;

        RaycastHit hitInfo; 
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, range)) {
            targetHit target = hitInfo.transform.GetComponent<targetHit>();
            if (target != null) {
                Debug.Log("hit enemy");
                target.takeDamage(damage);
            }
            
        }

    }
}
