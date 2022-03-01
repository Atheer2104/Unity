using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float rotation;
    public AudioSource audioSource;
    private  bool audioPlayed = false;
    private FadeOutAudio fadeOutAudio;

    public Camera cam;
    public Vector3 bulletTarget;
    public float health;
    public Text healthText;
    public float meteorDamage, fireballDamage;
    public LevelChanger levelChanger;

    public GameObject flameThrower;

        private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        healthText.text = "Health: " + health.ToString();
        fadeOutAudio = GetComponent<FadeOutAudio>();
    }
    private void FixedUpdate()
    {
        transform.Rotate(-(Input.GetAxis("Vertical") * rotation * Time.deltaTime), (Input.GetAxis("Horizontal") * rotation * Time.deltaTime), 0);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(0, 0, speed * Time.deltaTime * 100, ForceMode.VelocityChange);
            flameThrower.SetActive(true);
            if (!audioPlayed) {
                audioSource.Play(0);
                audioPlayed = true;
            }

        }
        else
        {
            flameThrower.SetActive(false);
            
            if (audioPlayed) {
            StartCoroutine(fadeOutAudio.fadeAudio(audioSource, 0.1f));
            audioPlayed = false;

            
            }   
        }

    }
    private void Update()
    {
        if (transform.position.x > 150 || transform.position.x <-150 || transform.position.y > 150 || transform.position.y <-150)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Meteor"))
        {
            health = health - meteorDamage;
            healthText.text = "Health: " + health.ToString();
            if (health <= 0)
            {
                levelChanger.fadeToLevel(2);
            }
            Destroy(collision.collider.gameObject, 0.07f);
        }
        if (collision.collider.CompareTag("Fireball"))
        {
            health = health - fireballDamage;
            healthText.text = "Health: " + health.ToString();
            if (health <= 0)
            {
                SceneManager.LoadScene(2);
            }
            Destroy(collision.collider.gameObject, 0.07f);
        }
    }
}