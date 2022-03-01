using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;

    public GameObject completeLevelUI;

    public void completeLevel() {
        completeLevelUI.SetActive(true);
    }

    public void EndGame() {

        if (gameHasEnded == false) {
            gameHasEnded = true;
            // restart game invoke method name string, time of delay
            Invoke("Restart", restartDelay);
        }
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   
}
