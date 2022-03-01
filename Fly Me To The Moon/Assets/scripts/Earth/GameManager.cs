using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int currentGold;
    public int amountOfGoldNeeded;
    public Text goldText;
    public bool hasSpecialPart = false;

    public void AddGold(int goldToAdd) {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold + "/" + amountOfGoldNeeded;
    }
}
