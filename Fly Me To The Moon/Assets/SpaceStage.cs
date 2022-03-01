using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStage : MonoBehaviour
{
    public GameObject trigger1;
    public GameObject trigger2;

    public GameObject trigger3;
    public GameObject spaceCylinder;

    public GameObject meteorSpawner, enemySpawner;

    public GameObject enemyParent;
    public GameObject text;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.name == "Space Trigger 1")
        {
            meteorSpawner.SetActive(true);
            trigger1.GetComponent<MeshRenderer>().enabled = false;
        }

        if (collider.gameObject.name == "Space Trigger 2")
        {
            meteorSpawner.SetActive(false);
            enemySpawner.SetActive(true);
            GetComponent<PlayerShoot>().enabled = true;
            Destroy(trigger2);
        }
        if (collider.gameObject.name == "Space Trigger 3")
        {
            enemyParent.SetActive(false);
            enemySpawner.SetActive(false);
            Destroy(trigger3);
            Destroy(spaceCylinder);
            text.SetActive(true);
            //Play Cutscene/Endgame
        }
    }
}
