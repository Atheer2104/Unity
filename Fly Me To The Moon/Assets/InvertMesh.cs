using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class InvertMesh : MonoBehaviour
{
    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
