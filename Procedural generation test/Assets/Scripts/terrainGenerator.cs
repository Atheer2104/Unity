    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainGenerator : MonoBehaviour {

    const float viewerMoveThresholdForChunkUpdate =  25f;
    const float sqrviewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public int colliderLevelOfDetailIndex;
    public levelOfDetailInfo[] detailLevels;

    public meshSettings meshSettings;
    public noiseMapSettings noiseMapSettings;
    public textureData textureSettings;

    public Transform viewer;
    public Material mapMaterial;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    float meshWorldmeshWorldSize;
    int chunkVisibleInViewDistance;

    // key vector2 for coordinate and corresponding terrain chunk
    Dictionary<Vector2, terrainChunk> terrainChunkDictionary = new Dictionary<Vector2, terrainChunk>();
    List<terrainChunk> visibleTerrainChunk = new List<terrainChunk>();

    void Start() {
        textureSettings.applyToMaterial(mapMaterial);
        textureSettings.updateMeshHeight(mapMaterial, noiseMapSettings.minHeight, noiseMapSettings.maxHeight);

        float maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;
        meshWorldmeshWorldSize = meshSettings.meshWorldSize;
        chunkVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / meshWorldmeshWorldSize);

        updateVisibleChunks();
    }

    void Update() {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if (viewerPosition != viewerPositionOld) {
            foreach (terrainChunk chunk in visibleTerrainChunk) {
                chunk.updateCollisinMesh();
            }
        }

        if ((viewerPositionOld-viewerPosition).sqrMagnitude > sqrviewerMoveThresholdForChunkUpdate) {
            viewerPositionOld = viewerPosition;
            updateVisibleChunks();
        }
        
    }

    void updateVisibleChunks() {
        // HashSet is represtent sets of values  
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (int i = visibleTerrainChunk.Count-1; i >= 0; i--) {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunk[i].coord);
            visibleTerrainChunk[i].updateTerrainChunk();
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x/meshWorldmeshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y/meshWorldmeshWorldSize);

        // staring from the left block and going toward the right block in y axis
        for (int yOffset = -chunkVisibleInViewDistance; yOffset <= chunkVisibleInViewDistance; yOffset++) {
            for (int xOffset = -chunkVisibleInViewDistance; xOffset <= chunkVisibleInViewDistance; xOffset++) {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) {
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord)) {
                        terrainChunkDictionary[viewedChunkCoord].updateTerrainChunk();
                    } else {
                        terrainChunk newChunk = new terrainChunk(viewedChunkCoord, noiseMapSettings, meshSettings, 
                        detailLevels, colliderLevelOfDetailIndex, transform, viewer, mapMaterial);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                        newChunk.onVisibilityChanged += onTerrainChunkVisibilityChanged;
                        newChunk.load();

                    }
                }
            } 
        }
    }

    void onTerrainChunkVisibilityChanged(terrainChunk chunk, bool isVisible) {
        if (isVisible) {
            visibleTerrainChunk.Add(chunk);
        } else {
            visibleTerrainChunk.Remove(chunk);
        }
    }

}

[System.Serializable]
public struct levelOfDetailInfo {
    [Range(0,meshSettings.numberOfSupportedLevelOfDetails-1)]
    public int levelOfDetail;
    public float visibleDistanceThreshold;

    public float sqrVisibleDistanceThreshold {
        get {
            return visibleDistanceThreshold * visibleDistanceThreshold;
        }
    }
}
