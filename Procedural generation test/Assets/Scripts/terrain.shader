Shader "Custom/terrain"
{   
    // this properties will be added to our meshMaterial
    Properties {
        testTexture("Texture", 2D) = "white"{}
        testScale("Scale", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxLayerCount = 8;
        const static float epsilon = 1E-4;

        int layerCount;
        // float3 because of RGB
        float3 baseColors[maxLayerCount];
        float baseStartHeights[maxLayerCount];
        float baseBlends[maxLayerCount];
        float baseColorStrength[maxLayerCount];
        float baseTextureScales[maxLayerCount];

        float minHeight;
        float maxHeight;

        sampler2D testTexture;
        float testScale;

        UNITY_DECLARE_TEX2DARRAY(baseTextures);

        struct Input {
            float3 worldPos;
            float worldNormal;
        };

        // float a is min value, float b is max value and value is current value 
        float inverseLerp(float a, float b, float value) {
            // saturate means clamp the value between 0 and 1 
            return saturate((value - a)/(b - a));
        }
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        float3 triplanar(float3 worldPos, float scale, float3 blendAxis, int textureIndex) {
            float3 scaledWorldPos = worldPos / scale;
            // tripleaner mapping 
            float3 xProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, 
            float3(scaledWorldPos.y, scaledWorldPos.z, textureIndex)) * blendAxis.x;

            float3 yProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, 
            float3(scaledWorldPos.x, scaledWorldPos.z, textureIndex)) * blendAxis.y;

            float3 zProjection = UNITY_SAMPLE_TEX2DARRAY(baseTextures, 
            float3(scaledWorldPos.x, scaledWorldPos.y, textureIndex)) * blendAxis.z;
            return xProjection + yProjection + zProjection;
        }

        // this function will be called for every pixel that our mesh is visible
        // we want to set the color at that surface 
        void surf (Input IN, inout SurfaceOutputStandard o) {

            float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            float3 blendAxis = abs(IN.worldNormal);
            blendAxis /= blendAxis.x + blendAxis.y + blendAxis.z;

            for (int i = 0; i < layerCount; i++) { 
                float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, (heightPercent - baseStartHeights[i]));
                float3 baseColor = baseColors[i] * baseColorStrength[i];
                float3 textureColor = triplanar(IN.worldPos, baseTextureScales[i], blendAxis, i) * (1-baseColorStrength[i]);
                // if drawStrength is 0 then we would set color to black 
                // but what we want is that if drawstength is 0 
                // then we want to use the same color, albedo * 1 + 0 will be same (what we want)
                o.Albedo = o.Albedo * (1-drawStrength) + (baseColor + textureColor) * drawStrength;
                
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
