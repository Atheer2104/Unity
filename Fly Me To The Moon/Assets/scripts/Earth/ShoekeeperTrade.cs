using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoekeeperTrade : MonoBehaviour {

    private bool triggerdEnter;
    public Image popupImage;
    public Text popupText;
    public float fadeSpeed;
    
    public GameManager gameManager;
    private FadeInAndOutUI fadeInAndOut;

    // Start is called before the first frame update
    void Start() {
        fadeInAndOut = gameManager.GetComponent<FadeInAndOutUI>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && triggerdEnter) {
            //Debug.Log("interacting");
            if (gameManager.currentGold < gameManager.amountOfGoldNeeded) {
                popupText.text = "You Currently have " + gameManager.currentGold + " gold pices, come back when you have " 
                + gameManager.amountOfGoldNeeded + " gold pieces";
                // setting up correct width and size for popupimage and text so they are proprinate
                popupImage.rectTransform.sizeDelta = new Vector2(320 ,120);
                popupText.rectTransform.sizeDelta = new Vector2(300, 100);
            } else {
                popupText.text = "It looks like you have the right amount of gold pieces";
                gameManager.hasSpecialPart = true;
                popupImage.rectTransform.sizeDelta = new Vector2(320 ,100);
                popupText.rectTransform.sizeDelta = new Vector2(300, 75);
            }

            StartCoroutine(fadeInAndOut.fadeInImage(popupImage, popupText, fadeSpeed));
        }
    }

    

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            triggerdEnter = true;
        }
    }

    private void OnTriggerExit() {
        triggerdEnter = false;
        StartCoroutine(fadeInAndOut.fadeOutImage(popupImage, popupText, fadeSpeed));
    }
}
