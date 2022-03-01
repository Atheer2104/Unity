using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public PlayerController playerController;
    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnTime;

    public GameObject deathEffect;
    private AudioSource audioSource;
    public Image blackScreen;
    private bool fadeToBlack;
    private bool fadeFromBlack;
    public float fadeSpeed;
    public float waitForFade;

    // Start is called before the first frame update    
    void Start() {
        currentHealth = maxHealth;
        // respawn point is where the player first start in the game 
        respawnPoint = playerController.transform.position;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (fadeToBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, 
            blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            
            if (blackScreen.color.a == 1f) {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, 
            blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            
            if (blackScreen.color.a == 0f) {
                fadeFromBlack = false;
            }
        }
        
    }

    public void respawn() {
 
        if (!isRespawning) {
            StartCoroutine("respawnCo");
        } 

    }

    public IEnumerator respawnCo() {
        
        isRespawning = true;
        playerController.gameObject.SetActive(false);
        Instantiate(deathEffect, playerController.transform.position, playerController.transform.rotation);
        audioSource.Play(0);

        yield return new WaitForSeconds(respawnTime);

        fadeToBlack = true;

        yield return new WaitForSeconds(waitForFade);
        
        fadeToBlack = false;
        fadeFromBlack = true;

        isRespawning = false;
        playerController.gameObject.SetActive(true);

        playerController.characterController.enabled = false;
        playerController.transform.position = respawnPoint;
        playerController.characterController.enabled = true;
        currentHealth = maxHealth;
        
    }

    public void hurtPlayer(int damage, Vector3 direction) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            respawn();
        } else {
            playerController.knockback(direction);
        }
    }

    public void healthPlayer(int healAmount) {
        currentHealth += healAmount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

}
