using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffsetFollow : MonoBehaviour
{
    public GameObject player;
    public float offset;
    public float endOffset;
    public float distance;
    public float speed;
    public Transform cylinderDisc1;
    public Transform cylinderDisc2;
    private bool hasMoved;
    
    void Start()
    {
        hasMoved = false;
    }
    void Update()
    {   
        if (hasMoved == false && transform.position != cylinderDisc2.position)
        {
        transform.position = Vector3.MoveTowards(transform.position, cylinderDisc2.position, speed * Time.deltaTime);
        }
        else if (transform.position == cylinderDisc2.position)
        {
            hasMoved = true;
        }
        
        if (hasMoved == true && transform.position != cylinderDisc1.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, cylinderDisc1.position, speed * Time.deltaTime);
        }
        else if (transform.position == cylinderDisc1.position)
        {
            hasMoved = false;
        }
    }
}
