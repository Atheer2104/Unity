using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public Animator animator;
    public int LevelToLoad;

    public void fadeToLevel(int LevelToLoad) {
        animator.SetTrigger("fadeOut");
        
    }

    public void onFadeComplete() {
        SceneManager.LoadScene(LevelToLoad);
    }

}
