using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rubiks : MonoBehaviour {

    GameObject[] cubeArray;
    int children;

    public Vector3[] downRotate = new Vector3[9];
    public Vector3[] topRotate = new Vector3[9];
    public Vector3[] rightRotate = new Vector3[9];
    public Vector3[] backsideRotate = new Vector3[9];


    public Vector3[] orangeCorners = new Vector3[4];
    // Start is called before the first frame update
    void Start() {
        GameObject rubiksCube = GameObject.FindGameObjectWithTag("cube");
        children = rubiksCube.transform.childCount;
        cubeArray = new GameObject[children];
        for (var i = 0; i < children; i++) {
            GameObject child = rubiksCube.transform.GetChild(i).gameObject;
            cubeArray[i] = child;
        }

    }

    private bool pressed = false;
    void Update() {
        
        if (Input.GetKeyDown("s") && !pressed) {
            GameObject rubiksCube = GameObject.FindGameObjectWithTag("cube");

            var rotator = new GameObject("Rotater");
            rotator.transform.position = new Vector3(0.9f, 0.9f, 0.9f);
            rotator.transform.parent = rubiksCube.transform;

            rotateCubes(downRotate, rotator, 0f, 90f, 0f, rubiksCube); 
        } else if (Input.GetKeyDown("d") && !pressed) {
            GameObject rubiksCube = GameObject.FindGameObjectWithTag("cube");

            var rotator = new GameObject("Rotater");
            rotator.transform.position = new Vector3(0.9f, 0.9f, 0.9f);
            rotator.transform.parent = rubiksCube.transform;
            
            rotateCubes(rightRotate, rotator, 90f, 0f, 0f, rubiksCube);            
        } 
    }

    private bool rotating;
    private IEnumerator rotate(GameObject gameObject, Vector3 angles, float duration, GameObject empty) {
        rotating = true;
        pressed = true;
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t+= Time.deltaTime) {
            gameObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }

        gameObject.transform.rotation = endRotation;
        rotating = false;
        pressed = false;

        //gameObject.transform.DetachChildren();
        //Destroy(gameObject);
        //parentAllCubes(cubeArray, empty);

        //Debug.Log(isCornerSolved("orange", orangeCorners));

    }

    public bool isCornerSolved(string tag, Vector3[] cubeorners) {
        GameObject[] corners = GameObject.FindGameObjectsWithTag("corner");
        int n = 0;
        for (var k = 0; k < corners.Length; k++) {
            GameObject firstCorner = corners[k]; 
            int len = firstCorner.transform.childCount;
            for (var i = 0; i < len; i++) {
                GameObject side = firstCorner.transform.GetChild(i).gameObject;
                if (side.tag == tag) {
                    foreach(Vector3 pos in cubeorners) {
                        if (side.transform.position.ToString() == pos.ToString()) {
                            n++;
                            if (n == 4) {
                                return true;
                            } 
                        }
                    }
                }
            }
        }
        return false;
    }

    

    public void startRotation(GameObject rotator, Vector3 angles, float duration, GameObject empty) {
        if (!rotating) {
            StartCoroutine(rotate(rotator, angles, duration, empty));
        }
    }

    private void parentAllCubes(GameObject[] array, GameObject emptyObj) {
        for (int i = 0; i < array.Length; i++) {
            array[i].transform.parent = emptyObj.transform;
        }
    }

    private void rotateCubes (Vector3[] typeOfRotation, GameObject rotator, float x, float y, float z, GameObject empty) {
        foreach(Vector3 pos in typeOfRotation) {
            for (var i = 0; i < children; i++) {
                if (cubeArray[i].transform.position.ToString() == pos.ToString()) {
                   cubeArray[i].transform.parent = rotator.transform;
                }
            } 
        }

        startRotation(rotator, new Vector3(x, y, z), 1, empty);
    }



}
