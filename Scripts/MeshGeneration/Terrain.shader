Shader "Custom/Terrain"
{
    Properties
    {
        testTexture("Texture", 2D) = "white" {}
        testScale("Scale", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5



            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            sampler2D testTexture;
            float testScale;

            const static int maxLayerCount = 8;
            const static float epsilon = 1E-4;

            float minHeight;
            float maxHeight;
            int layerCount;
            float4 baseColors[maxLayerCount];
            float baseStartHeights[maxLayerCount];
            float baseBlends[maxLayerCount];
            float baseColorStrength[maxLayerCount];
            float baseTextureScales[maxLayerCount];

            TEXTURE2D_ARRAY(baseTextures);
            SAMPLER(sampler_baseTextures);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos    : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(worldPos);
                OUT.worldPos = worldPos;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            float inverseLerp(float a, float b, float value)
            {
                return saturate((value - a) / (b - a));
            }

            float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex) 
            {
                float3 scaledWorldPos = worldPos / scale;
                
                float3 xProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, float2(scaledWorldPos.y, scaledWorldPos.z), textureIndex).rgb * blendAxes.x;
                float3 yProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, float2(scaledWorldPos.x, scaledWorldPos.z), textureIndex).rgb * blendAxes.y;
                float3 zProjection = SAMPLE_TEXTURE2D_ARRAY(baseTextures, sampler_baseTextures, float2(scaledWorldPos.x, scaledWorldPos.y), textureIndex).rgb * blendAxes.z;

                return xProjection + yProjection + zProjection;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
                float3 finalColor = float3(0, 0, 0); // default white
                float3 blendAxes = abs(IN.normalWS);
                // blendAxes /= blendAxes.x + blendAxes.y + blendAxes.z;

                // Height-based blending
                for (int i = 0; i < layerCount; i++)
                {
                    float drawStrength = inverseLerp(-baseBlends[i] / 2 - epsilon, baseBlends[i] / 2, heightPercent - baseStartHeights[i]);

                    float3 baseColor = baseColors[i] * baseColorStrength[i];
                    float3 textureColor = triplanar(IN.worldPos, baseTextureScales[i], blendAxes, i) * (1 - baseColorStrength[i]);

                    finalColor = finalColor * (1 - drawStrength) + (baseColor + textureColor) * drawStrength;
                }

                //finalColor = xProjection + yProjection + zProjection;

                return float4(finalColor, 1);
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
