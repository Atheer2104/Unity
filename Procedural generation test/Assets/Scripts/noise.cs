using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class noise {

    // local for using local min and max height 
    // global for estimiting min and max height
    public enum NormalizeMode {Local, Global};
    
    public static float[,] generateNoiseMap(int mapWidth, int mapHeight, noiseSettings settings, Vector2 sampleCentre) {
       // creating our noisemap with diemensions our map size 
       float[,] noiseMap = new float[mapWidth, mapHeight];

       System.Random prng = new System.Random(settings.seed);
       Vector2[] octavesOffsets = new Vector2[settings.octaves];

       float maxPossibleHeight = 0; 
       float amplitude = 1;
       float frequency = 1;

       for (int i = 0; i < settings.octaves; i++) {
           // using the seed is what makes us get unique noisemaps 
           float offsetX = prng.Next(-100000, 100000) + settings.offset.x + sampleCentre.x;
           float offsetY = prng.Next(-100000, 100000) - settings.offset.y - sampleCentre.y;
           octavesOffsets[i] = new Vector2(offsetX, offsetY);
           
           maxPossibleHeight += amplitude;
           amplitude *= settings.persistance;
       }

       float maxLocalNoiseHeight = float.MinValue;
       float minLocalNoiseHeight = float.MaxValue;

       // getting half of the map so when we update scale so when 
       // we zoom in the center instead of top right corner 
       float halfWidth = mapWidth/2f;
       float halfHeight = mapHeight/2f;

       for (int y = 0; y < mapHeight; y++) {
           for (int x = 0; x < mapWidth; x++) {

               amplitude = 1;
               frequency = 1;
               float noiseHeight = 0;

               // the higher the octaves is the more fine detail we get 
               for (int i = 0; i < settings.octaves; i++) {
                    // getting float values and we use them to create our perlin noise
                    // the higher the frequency means the height values change more rapidly 
                    float sampleX = (x - halfWidth + octavesOffsets[i].x) / settings.scale * frequency;
                    float sampleY = (y - halfHeight + octavesOffsets[i].y) / settings.scale * frequency;

                    // addidtional calculation is used so we could get negative values 
                    // which reduces the noiseHeight
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    // persistance is range for 0 to 1 by looping over amplitude will decrease
                    amplitude *= settings.persistance;
                    // frequency will increase over loops 
                    frequency *= settings.lacunarity;

               }

                if (noiseHeight > maxLocalNoiseHeight) {
                    maxLocalNoiseHeight = noiseHeight;
                } 
                if (noiseHeight < minLocalNoiseHeight) {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

                if (settings.normalizeMode == NormalizeMode.Global) {
                    // if want less mountain part then 2f * maxPossibleHeight / 1.75f is good value 
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f); 
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
           }
       }
        if (settings.normalizeMode == NormalizeMode.Local) {
            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) { 
                        // making sure that noise map is a value between 0 and 1
                        // because earlier it could have negative values
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    }   
                }
            }

       return noiseMap;
   }
    
}

[System.Serializable]
public class noiseSettings {
    public noise.NormalizeMode normalizeMode;

    public float scale = 50f;

    public int octaves = 6;
    // making peristance value a slider with range of 0 to 1 
    [Range(0,1)]
    public float persistance = 0.6f;
    public float lacunarity = 2;  

    public int seed;
    public Vector2 offset;

    // making sure that our values like scale 
    // and so on is in a valid range
    public void validateValues() {
        // is scale is less than 0.01f then the scale value will be it
        scale = Mathf.Max(scale, 0.01f);
        octaves = Mathf.Max(octaves, 1);
        lacunarity = Mathf.Max(lacunarity, 1);
        persistance = Mathf.Clamp01(persistance);
    }

}