using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAudio : MonoBehaviour {

    public IEnumerator fadeAudio(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume> 0) {
            audioSource.volume -= startVolume * (Time.deltaTime/fadeTime);

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}
