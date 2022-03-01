using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class textureGenerator {
   
    public static Texture2D textureFromColorMap(Color[] colorMap, int width, int height) {
        // creating a new 2D texture 
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(noiseMap noiseMap) {
         // getting first dimension from our noisemap which is the width
        int width = noiseMap.values.GetLength(0);
        // getting the second diemension from our NoiseMap
        int height = noiseMap.values.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                // we get row we currently on by multiplying y and width
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(noiseMap.minValue, 
                noiseMap.maxValue,noiseMap.values[x, y]));
            }
        }

        return textureFromColorMap(colorMap, width, height);
    }

}
