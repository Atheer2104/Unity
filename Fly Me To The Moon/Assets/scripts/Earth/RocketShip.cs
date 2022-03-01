using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour {

    private bool triggerdEnter;
    public GameManager gameManager;
    public Image popupImage;    
    public Image SceneFadeImage;
    public Text popupText;
    public float fadeSpeed;
    private FadeInAndOutUI fadeInAndOut;
    public float countUntilNextSceen;
    public int sceneToLoad;
    public LevelChanger levelChanger;

    // Start is called before the first frame update
    void Start() {
        fadeInAndOut = gameManager.GetComponent<FadeInAndOutUI>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.E) && triggerdEnter) {
            //Debug.Log("Interacting");
            
            if (gameManager.hasSpecialPart) {
                popupText.text = "Now the rocket can launch with missing part that you collected. Prepare for launch 3...2...1";
                popupImage.rectTransform.sizeDelta = new Vector2(320 ,150);
                popupText.rectTransform.sizeDelta = new Vector2(300, 140);

                StartCoroutine(NextScene(countUntilNextSceen));
            } else {
                popupText.text = "You need a special part for the rocket to be able to fly." 
                + " I have heard that the shoe keeper has the part we need.";
                // setting up correct width and size for popupimage and text so they are proprinate
                popupImage.rectTransform.sizeDelta = new Vector2(320 ,170);
                popupText.rectTransform.sizeDelta = new Vector2(300, 165);
            }

            StartCoroutine(fadeInAndOut.fadeInImage(popupImage, popupText, fadeSpeed));
            
        }
    }

    public IEnumerator NextScene(float delay) {
        yield return new WaitForSeconds(delay/1.2f);

        StartCoroutine(fadeInAndOut.fadeOutImage(popupImage, popupText, fadeSpeed));

        yield return new WaitForSeconds(delay-(delay/1.2f));

        levelChanger.fadeToLevel(sceneToLoad);
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
