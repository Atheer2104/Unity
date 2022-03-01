using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOutUI : MonoBehaviour {
    
    public IEnumerator fadeInImage(Image popupImage, Text popupText, float fadeSpeed) {
        while (popupImage.color.a < 1.0f) {
            popupImage.color = new Color(popupImage.color.r, popupImage.color.g, popupImage.color.b, popupImage.color.a + (Time.deltaTime / fadeSpeed));
            popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, popupText.color.a + (Time.deltaTime / fadeSpeed));
            yield return null;
        }  
    }

    public IEnumerator fadeOutImage(Image popupImage, Text popupText, float fadeSpeed) {
        while (popupImage.color.a > 0.0f) {
            popupImage.color = new Color(popupImage.color.r, popupImage.color.g, popupImage.color.b, popupImage.color.a - (Time.deltaTime / fadeSpeed));
            popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, popupText.color.a - (Time.deltaTime / fadeSpeed));
            yield return null;
        }  
    }
}
