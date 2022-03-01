using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainChunk {
        
        // a value that determines how close the player has to be at the edge of chunk 
        const float colliderGenerationDistanceThreshold = 5f;
        public event System.Action<terrainChunk, bool> onVisibilityChanged;
        public Vector2 coord;

        GameObject meshObject;
        Vector2 sampleCentre;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        levelOfDetailInfo[] detailsLevels;
        levelOfDetialMesh[] levelOfDetailMeshs;
        int colliderLevelOfDetailIndex;

        noiseMap noiseMap;
        bool noiseMapRecived;
        int previousLevelOfDetailIndex = -1;
        bool hasSetCollider;
        float maxViewDistance;

        noiseMapSettings noiseMapSettings;
        meshSettings meshSettings;        
        Transform viewer;

        public terrainChunk(Vector2 coord, noiseMapSettings noiseMapSettings, meshSettings meshSettings, 
        levelOfDetailInfo[] detailsLevels, int colliderLevelOfDetailIndex,Transform parent, 
        Transform viewer, Material material) {
            this.coord = coord;
            this.detailsLevels = detailsLevels;
            this.colliderLevelOfDetailIndex = colliderLevelOfDetailIndex;
            this.noiseMapSettings = noiseMapSettings;
            this.meshSettings = meshSettings;
            this.viewer = viewer;

            // we earlier specified that our coord is our sampleCentre ex. -240,0 which means a block to left 
            // and we get our coord by dividing with our meshWorldmeshWorldSize which is 240 and we get coord
            // -1,0 and now we are getting sampleCentre with same formula
            sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
            Vector2 position = coord * meshSettings.meshWorldSize;
            bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);
            

            meshObject = new GameObject("Terrain chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = new Vector3(position.x, 0, position.y);
            meshObject.transform.parent = parent;
            // by default the chunk is not visible but updateTerrainChunk method will determine 
            // if the chunk should be visible 
            setVisible(false);

            levelOfDetailMeshs = new levelOfDetialMesh[detailsLevels.Length];
            for (int i = 0; i< detailsLevels.Length; i++) {
                levelOfDetailMeshs[i] = new levelOfDetialMesh(detailsLevels[i].levelOfDetail);
                levelOfDetailMeshs[i].updateCallback += updateTerrainChunk;
                if (i == colliderLevelOfDetailIndex) {
                    levelOfDetailMeshs[i].updateCallback += updateCollisinMesh;
                }
            }

            maxViewDistance = detailsLevels[detailsLevels.Length-1].visibleDistanceThreshold;

            
        }

        public void load() {
            // () => function return a faster to declare a methood that takes no paramaters
            threadedDataRequester.requestData(() =>  noiseMapGenerator.GenerateNoiseMap(meshSettings.NumberOfVerticesPerLine, 
            meshSettings.NumberOfVerticesPerLine, noiseMapSettings, sampleCentre), onNoiseMapRecived);
        }

        void onNoiseMapRecived(object noiseMapObject) { 
            this.noiseMap = (noiseMap) noiseMapObject;
            noiseMapRecived = true;

            updateTerrainChunk();
        }

        Vector2 viewerPosition {
            get {
                return new Vector2(viewer.position.x, viewer.position.z);
            }
        }

        public void updateTerrainChunk() {
            if (noiseMapRecived) {
                // return the smallest sqrDistance between a given point and the bounds location
                float playerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                
                bool wasVisible = isVisible(); 
                bool visible = playerDistanceFromNearestEdge <= maxViewDistance;

                if (visible) {
                    int levelOfDetailIndex = 0;
                    
                    for (int i = 0; i < detailsLevels.Length-1; i++) {
                        if (playerDistanceFromNearestEdge > detailsLevels[i].visibleDistanceThreshold) {
                            levelOfDetailIndex = i+1;
                        }else {
                            break;
                        }
                    }
                    
                    if (levelOfDetailIndex != previousLevelOfDetailIndex) {
                        levelOfDetialMesh levelOfDetialMesh = levelOfDetailMeshs [levelOfDetailIndex];
                        if (levelOfDetialMesh.hasMesh) {
                            previousLevelOfDetailIndex = levelOfDetailIndex;
                            meshFilter.mesh = levelOfDetialMesh.mesh;
                        }else if (!levelOfDetialMesh.hasRequestedMesh) { 
                            levelOfDetialMesh.requestMesh(noiseMap, meshSettings);

                        }
                    }
                }

                // chunks visibilty has changed 
                if (wasVisible != visible) {
                    setVisible(visible);
                    if (onVisibilityChanged != null) {
                        onVisibilityChanged(this, visible);
                    }
                }
                
            }
            
        }

        public void updateCollisinMesh() {
            float sqrDistanceFromPlayerToEdge = bounds.SqrDistance(viewerPosition);
            
            if (!hasSetCollider) {
                if (sqrDistanceFromPlayerToEdge < detailsLevels[colliderLevelOfDetailIndex].sqrVisibleDistanceThreshold) {
                    if (!levelOfDetailMeshs[colliderLevelOfDetailIndex].hasRequestedMesh) {
                        levelOfDetailMeshs[colliderLevelOfDetailIndex].requestMesh(noiseMap, meshSettings);
                    }
                }
            }
            

            if (sqrDistanceFromPlayerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
                if (levelOfDetailMeshs[colliderLevelOfDetailIndex].hasMesh) {
                    meshCollider.sharedMesh = levelOfDetailMeshs[colliderLevelOfDetailIndex].mesh;
                    hasSetCollider = true;
                }
                
            }
        }

        public void setVisible(bool visible) {
            meshObject.SetActive(visible);
        }

        public bool isVisible() {
            return meshObject.activeSelf;
        }
    }

    class levelOfDetialMesh {

        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int levelOfDetail;
        public event System.Action updateCallback;

        public levelOfDetialMesh(int levelOfDetail) {
            this.levelOfDetail = levelOfDetail;
        }

        void onMeshDataRecived(object meshDataObject) {
            mesh = ((meshData) meshDataObject).createMesh();
            hasMesh = true;
            
            updateCallback();
        }

        public void requestMesh(noiseMap noiseMap, meshSettings meshSettings) {
            hasRequestedMesh = true;
            threadedDataRequester.requestData(() => meshGenerator.generateTerrainMesh(noiseMap.values, meshSettings, levelOfDetail)
            , onMeshDataRecived);
        }
        
    }
