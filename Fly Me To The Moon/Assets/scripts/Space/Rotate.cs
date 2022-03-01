using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public bool isMeteor;
    public float maxRotateSpeed;

    private void Start()
    {
        if (isMeteor)
        {
            rotationX = Random.Range(0, maxRotateSpeed);
            rotationY = Random.Range(0, maxRotateSpeed);
            rotationZ = Random.Range(0, maxRotateSpeed);
        }
    }
    void Update()
    {
        transform.Rotate(new Vector3(rotationX, rotationY, rotationZ) * Time.deltaTime);
    }
}

