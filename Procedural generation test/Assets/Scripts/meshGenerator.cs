using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshGenerator {

    public static meshData generateTerrainMesh(float[,] noisemap, meshSettings meshSettings ,int levelOfDetail) {
        int skipIncreament = (levelOfDetail == 0)?1 : levelOfDetail * 2;
        int numberVerticesPerLine = meshSettings.NumberOfVerticesPerLine;
        
        // this calculations is allowing to center our mesh 
        Vector2 topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

        meshData meshData = new meshData(numberVerticesPerLine, skipIncreament, meshSettings.useFlatShading);
        
        int[,] vertexIndicesMap = new int[numberVerticesPerLine, numberVerticesPerLine];
        int meshVertexIndex = 0;
        int outOfMeshVertexIndex = -1;

         for (int y = 0; y < numberVerticesPerLine; y++) {
            for (int x = 0; x < numberVerticesPerLine; x++) {
                bool isOutOfMeshVertex = y == 0 || y == numberVerticesPerLine - 1 || x == 0 || x == numberVerticesPerLine - 1;
                bool isSkippedVertedx = x > 2 && x < numberVerticesPerLine-3 && y > 2 && y < numberVerticesPerLine-3 && 
                ((x-2)%skipIncreament != 0 || (y-2)% skipIncreament !=0);

                if (isOutOfMeshVertex) {
                   vertexIndicesMap [x, y] = outOfMeshVertexIndex;
				   outOfMeshVertexIndex--;
                } else if (!isSkippedVertedx){
                    vertexIndicesMap[x,y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
         }

        for (int y = 0; y < numberVerticesPerLine; y++) {
            for (int x = 0; x < numberVerticesPerLine; x++) {
                bool isSkippedVertedx = x > 2 && x < numberVerticesPerLine-3 && y > 2 && y < numberVerticesPerLine-3 && 
                ((x-2)%skipIncreament != 0 || (y-2)% skipIncreament !=0);

                if (!isSkippedVertedx) {

                    bool isOutOfMeshVertex = y == 0 || y == numberVerticesPerLine - 1 || x == 0 || x == numberVerticesPerLine - 1;
                    bool isMeshEdgeVertex = (y == 1 || y == numberVerticesPerLine-2 || x == 1 || x == numberVerticesPerLine-2) && 
                    !isOutOfMeshVertex;
                    bool isMainVertex = (x-2)%skipIncreament == 0 && (y-2)%skipIncreament == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
                    bool isEdgeConnectionVertex = (y== 2 || y== numberVerticesPerLine-3 || x == 2 || x == numberVerticesPerLine-3) &&
                    !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

                    int vertexIndex = vertexIndicesMap[x, y];
                    // uvs/percent keep tracks of each vertex where it is in 
                    // realtion to rest of the map in percenatge that ranges 0 to 1 
                    Vector2 percent = new Vector2(x-1, y-1)/(numberVerticesPerLine -3);
                    Vector2 vertexPosition2D = topLeft + new Vector2(percent.x,-percent.y) * meshSettings.meshWorldSize;
                    float height = noisemap[x, y]; 
                    
                    if (isEdgeConnectionVertex) {
                        bool isVertical = x == 2 || x==numberVerticesPerLine - 3;
                        // the main vertex above it
                        int distanceToMainVertexA = ((isVertical)? y-2: x-2)%skipIncreament;
                        // the main vertex below it
                        int distanceToMainVertexB = skipIncreament - distanceToMainVertexA;
                        float distancePercentFromAToB = distanceToMainVertexA / (float)skipIncreament;

                        float heightMainVertexA = noisemap [(isVertical)?x : x-distanceToMainVertexA, 
                        (isVertical)? y-distanceToMainVertexA : y];

                        float heightMainVertexB = noisemap [(isVertical)?x : x+distanceToMainVertexB, 
                        (isVertical)? y+distanceToMainVertexB : y];

                        height = heightMainVertexA * (1-distancePercentFromAToB) + heightMainVertexB * distancePercentFromAToB;
                    }

                    meshData.addVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

                    bool createTriangle = x < numberVerticesPerLine-1 && y < numberVerticesPerLine-1 && 
                    (!isEdgeConnectionVertex || (x != 2 && y != 2));

                    // here we are ignoring the right and bottom side of our vertices 
                    // because there we won't create any triangles 
                    if (createTriangle) {
                        int currentIncreament = (isMainVertex && x != numberVerticesPerLine-3 && y != numberVerticesPerLine-3)?skipIncreament : 1;

                        int a = vertexIndicesMap[x,y];
                        int b = vertexIndicesMap[x + currentIncreament, y];
                        int c = vertexIndicesMap[x, y + currentIncreament];
                        int d = vertexIndicesMap[x + currentIncreament, y + currentIncreament];
                        meshData.addTriangle(a,d,c);
                        meshData.addTriangle(d,a,b); 
                    }
                }
            }
        }

        // making calculate normals run on a diffierent thread
        meshData.processMesh();

        return meshData;
    }
}

public class meshData {
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] bakedNormals;

    Vector3[] outOfMeshVertex;
    int[] outOfMeshTriangle;

    int triangleIndex;
    int outOfmeshTriangleIndex;

    bool useFlatShading;

    public meshData(int numberVerticesPerLine, int skipIncreament ,bool useFlatShading) {
        this.useFlatShading = useFlatShading;

        int numberMeshEdgeVertex = (numberVerticesPerLine-2) * 4 - 4;
        int numberEdgeConnectionVertex = (skipIncreament-1) * (numberVerticesPerLine-5) / skipIncreament * 4;
        int numberMainVertexPerLine = (numberVerticesPerLine-5) / skipIncreament + 1;
        int numberMainVertex = numberMainVertexPerLine * numberMainVertexPerLine;

        vertices = new Vector3[numberMeshEdgeVertex + numberEdgeConnectionVertex + numberMainVertex];
        uvs = new Vector2[vertices.Length];

        int numberMeshEdgeTriangles = 8 * (numberVerticesPerLine - 4);
        int numberMainTriangles = (numberMainVertexPerLine-1) * (numberMainVertexPerLine-1) * 2;
        triangles = new int[(numberMeshEdgeTriangles + numberMainTriangles) * 3];

        outOfMeshVertex = new Vector3[numberVerticesPerLine * 4 - 4];
        outOfMeshTriangle = new int[24 * (numberVerticesPerLine-2)];
    }

    public void addVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex) {
        if (vertexIndex < 0) {
            // starting at zero, because it start at (-1)
            outOfMeshVertex[-vertexIndex-1] = vertexPosition;
        } else {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }
    }

    public void addTriangle(int a, int b, int c) {
        // a border
        if (a < 0 || b < 0 || c < 0) {
            outOfMeshTriangle[outOfmeshTriangleIndex] = a;
            outOfMeshTriangle[outOfmeshTriangleIndex+1] = b;
            outOfMeshTriangle[outOfmeshTriangleIndex+2] = c;
            // we have added three triangles, and updating triangleIndex accordingly
            outOfmeshTriangleIndex += 3;
        }else {
            triangles[triangleIndex] = a;
            triangles[triangleIndex+1] = b;
            triangles[triangleIndex+2] = c;
            // we have added three triangles, and updating triangleIndex accordingly
            triangleIndex += 3;
        }
        
    }

    
    Vector3[] calculateNormals() {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        // each triangle in triangles is a set of 3 triangles
        int triangleCount = triangles.Length/3;
        for (int i = 0; i< triangleCount; i++) {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex+1];
            int vertexIndexC = triangles[normalTriangleIndex+2];

            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        int boarderTriangleCount = outOfMeshTriangle.Length/3;
        for (int i = 0; i < boarderTriangleCount; i++) {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = outOfMeshTriangle[normalTriangleIndex];
            int vertexIndexB = outOfMeshTriangle[normalTriangleIndex+1];
            int vertexIndexC = outOfMeshTriangle[normalTriangleIndex+2];

            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            if (vertexIndexA >= 0) {
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0) {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0) {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }

        for (int i = 0; i < vertexNormals.Length; i++) {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    Vector3 surfaceNormalFromIndices(int indexA, int indexB, int indexC) {
        Vector3 pointA = (indexA < 0)?outOfMeshVertex[-indexA-1] : vertices[indexA];
        Vector3 pointB = (indexB < 0)?outOfMeshVertex[-indexB-1] : vertices[indexB];
        Vector3 pointC = (indexC < 0)?outOfMeshVertex[-indexC-1] : vertices[indexC];

        //cross product vector
        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;

    }

    public void processMesh() {
        if (useFlatShading) {
            flatShading();
        }else {
            BakedNormals();
        }
    }

    private void BakedNormals() {
        bakedNormals = calculateNormals();
    }

    void flatShading() {
        Vector3[] flatshadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for (int i = 0; i < triangles.Length; i++) {
            // flatshadedVertices it's the vertex from our current triangle
            flatshadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];   
            // updating triangle index 
            triangles[i] = i;
        }

        vertices = flatshadedVertices;
        uvs = flatShadedUvs;

    }

    public Mesh createMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlatShading) {
            mesh.RecalculateNormals();
        } else {
            mesh.normals = bakedNormals;
        }
        return mesh;    
    }

}
 