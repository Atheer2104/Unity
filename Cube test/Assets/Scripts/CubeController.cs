using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CubeController : MonoBehaviour {

    public List<GameObject> CubePrefabList;
    public Vector3 GridSize;
    GameObject[,,] cubeArray = new GameObject[3,3,3];

    public Vector3[] downRotate = new Vector3[9];
    public Vector3[] middle = new Vector3[9];
    public Vector3[] topRotate = new Vector3[9];
    public Vector3[] leftRotate = new Vector3[9];
    public Vector3[] rightRotate = new Vector3[9];
    public Vector3[] backsideRotate = new Vector3[9];
    public Vector3[] forwardsideRotate = new Vector3[9];

    public void generateColoredCubeSquare() {
        for (int x = 0; x < GridSize.x; x++) {
            for (int z = 0; z < GridSize.z; z++) {
                for (int y = 0; y < GridSize.y; y++) {
                    cubeArray[x, y, z] = Instantiate(CubePrefabList[Random.Range(0, CubePrefabList.Count)], new Vector3(x*1.08f, y*1.08f, z*1.08f), Quaternion.identity);
                    if (cubeArray[x, y, z].transform.position == new Vector3(1.08f, 0f, 1.08f)) {
                        Destroy(cubeArray[x, y, z]);
                        cubeArray[x, y, z] = Instantiate(CubePrefabList[0], new Vector3(1.08f, 0f, 1.08f), Quaternion.identity);

                    } else if (cubeArray[x, y, z].transform.position == new Vector3(1.08f, 1.08f, 1.08f)) {
                        Destroy(cubeArray[x, y, z]);
                        cubeArray[x, y, z] = Instantiate(CubePrefabList[1], new Vector3(1.08f, 1.08f, 1.08f), Quaternion.identity);
                        
                    } else if (cubeArray[x, y, z].transform.position == new Vector3(1.08f, 2.16f, 1.08f)) {
                        Destroy(cubeArray[x, y, z]);
                        cubeArray[x, y, z] = Instantiate(CubePrefabList[2], new Vector3(1.08f, 2.16f, 1.08f), Quaternion.identity);
                        
                    }
                } 
            }
        }
    }

    public void Start() {
        generateColoredCubeSquare();
        var empty = new GameObject("rubik's cube");
        empty.transform.position = new Vector3(0f, 0f, 0f);

        parentAllCubes(cubeArray, empty);
    }

     private bool pressed = false;
     void Update() {
         if (Input.GetKeyDown("s") && !pressed) {
            var empty = GameObject.Find("rubik's cube");

            var rotator = new GameObject("Rotater");
            rotator.transform.position = new Vector3(1.08f, 1.08f, 1.08f);
            rotator.transform.parent = empty.transform;
            
            rotateCubes(downRotate, cubeArray, rotator, 0f, 90f, 0f,empty); 

        } else if (Input.GetKeyDown("d") && !pressed) {
            var empty = GameObject.Find("rubik's cube");

            var rotator = new GameObject("Rotater");
            rotator.transform.position = new Vector3(1.08f, 1.08f, 1.08f);
            rotator.transform.parent = empty.transform;
            
            rotateCubes(rightRotate, cubeArray, rotator, 90f, 0f, 0f, empty);              
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

        gameObject.transform.DetachChildren();
        Destroy(gameObject);
        parentAllCubes(cubeArray, empty);

        if (isFacedSolved("green", downRotate) && isFacedSolved("yellow", middle) && isFacedSolved("red", topRotate)) {
            Debug.LogError("something is wrong it seems that you have solved the cube");
        }

    }

    public bool isFacedSolved(string tag, Vector3[] position) {
        GameObject[] colorCubes = GameObject.FindGameObjectsWithTag(tag);
        int k = 0;

        for (int i = 0; i < colorCubes.Length; i++) {
            for (int j = 0; j < position.Length; j++) {    
                if (colorCubes[i].transform.position == position[j]) {
                    k += 1;
                    if (k == 9) {
                        return true;
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

    private void parentAllCubes(GameObject[,,] array, GameObject emptyObj) {
        for (int i = 0; i < array.GetLength(0); i++) {
            for (int j = 0; j < array.GetLength(1); j++) {
                for (int k = 0; k < array.GetLength(2); k++) {
                    array[i, j, k].transform.parent = emptyObj.transform;
                }
            }
        }
    }

    private void rotateCubes (Vector3[] typeOfRotation, GameObject[,,] array, GameObject rotator, float x, float y, float z, GameObject empty) {
        for (int i = 0; i < array.GetLength(0); i++) {
            for (int j = 0; j < array.GetLength(1); j++) {
                for (int k = 0; k < array.GetLength(2); k++) {
                    
                    foreach(Vector3 pos in typeOfRotation) {
                        if (cubeArray[i, j, k].transform.position == pos) {
                            cubeArray[i, j, k].transform.parent = rotator.transform;
                        }
                    }
                }
            }
        }

        startRotation(rotator, new Vector3(x, y, z), 1, empty);
    }


}
