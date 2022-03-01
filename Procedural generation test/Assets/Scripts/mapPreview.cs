using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapPreview : MonoBehaviour {

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public enum DrawMode {values, mesh, falloffMap};
    public DrawMode drawMode;

    public meshSettings meshSettings;
    public noiseMapSettings noiseMapSettings;
    public textureData textureData;

    public Material terrainMaterial;

    [Range(0,meshSettings.numberOfSupportedLevelOfDetails-1)]
    public int EditorPreviewLevelOfDetail;
    public bool autoUpdate;


    public void drawMapInEditor() {
        textureData.applyToMaterial(terrainMaterial);
        textureData.updateMeshHeight(terrainMaterial, noiseMapSettings.minHeight, noiseMapSettings.maxHeight);
        noiseMap noiseMap = noiseMapGenerator.GenerateNoiseMap(meshSettings.NumberOfVerticesPerLine
        , meshSettings.NumberOfVerticesPerLine, noiseMapSettings, Vector2.zero);

        if (drawMode == DrawMode.values) {
            drawTexture(textureGenerator.TextureFromHeightMap(noiseMap));
        } else if (drawMode == DrawMode.mesh) {
            drawMesh(meshGenerator.generateTerrainMesh(noiseMap.values, meshSettings, EditorPreviewLevelOfDetail));
        }else if (drawMode == DrawMode.falloffMap) {
            drawTexture(textureGenerator.TextureFromHeightMap(new noiseMap(falloffGenerator.generateFalloffMap(meshSettings.NumberOfVerticesPerLine),0,1)));
        }
    }

    
    

    public void drawTexture(Texture2D texture) {
        // sharedMaterial is the material that is in the scene mode
        // normal matarial is excuated in playmode 
        // this gives access to preview our mapGeneration while we are at scene
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

        textureRenderer.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

    public void drawMesh(meshData meshData) {
        meshFilter.sharedMesh = meshData.createMesh();

         textureRenderer.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }
    

    void onValuesUpdated() {
        if (!Application.isPlaying) {
            drawMapInEditor();
        }
    }

    void onTextureValuesUpdated() {
        textureData.applyToMaterial(terrainMaterial);
    }

    
    // called automatically when a script varible is changed in the inspector
    void OnValidate() {
        
        
        if (meshSettings != null) {
            meshSettings.onValuesUpdated -= onValuesUpdated;
            meshSettings.onValuesUpdated += onValuesUpdated;
        }

        if (noiseMapSettings != null) {
            // we unscribe because we don't want to resubscribe all time eventually 
            //onValuesUpdated in this file will get called multiple times
            noiseMapSettings.onValuesUpdated -= onValuesUpdated;
            noiseMapSettings.onValuesUpdated += onValuesUpdated;
        }

        if (textureData != null) {
            textureData.onValuesUpdated -= onTextureValuesUpdated;
            textureData.onValuesUpdated += onTextureValuesUpdated;
        }
    }


}
