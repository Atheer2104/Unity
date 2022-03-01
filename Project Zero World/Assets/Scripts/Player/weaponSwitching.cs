using UnityEngine;

public class weaponSwitching : MonoBehaviour {

    // 0 rifle, 1 seconday/melee, 2 potion, 3 and 4 healing items
    public int selectedWeapon = 0;

    public bool hasGun;

    // Start is called before the first frame update
    void Start() {
        selectWeapon();
    }

    // Update is called once per frame
    void Update() {

        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedWeapon = 0;
        }


        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) {
            selectedWeapon = 2;
        }

        if (previousSelectedWeapon != selectedWeapon) {
            selectWeapon();
        }

        
    }

    void selectWeapon() {
        
        int i = 0;
        foreach (Transform weapon in transform) {
            if (i == selectedWeapon) {
                weapon.gameObject.SetActive(true);
                weapon.gameObject.tag = "Active";
                if (weapon.gameObject.GetComponent<gun>() != null)
                {
                    hasGun = true;
                }
                else
                {
                    hasGun = false;
                }
            } else {
                weapon.gameObject.SetActive(false);
                weapon.gameObject.tag = "Unactive";
            }
            i++;
        }
    }
}
