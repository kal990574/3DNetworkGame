Shader "Custom/ScanPulse"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "ScanPulsePass"
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float3 _ScanOrigin;
            float  _ScanRadius;
            float  _ScanWidth;
            float  _ScanIntensity;
            float4 _ScanColor;
            float  _ScanFalloff;
            float3 _ScanDirection;
            float  _ScanAngle;
            float  _ScanAngleSoftness;

            float4 Frag(Varyings input) : SV_Target
            {
                float4 sceneColor = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord);

                float depth = SampleSceneDepth(input.texcoord);
                float3 worldPos = ComputeWorldSpacePosition(input.texcoord, depth, UNITY_MATRIX_I_VP);

                float dist = distance(worldPos, _ScanOrigin);

                float halfWidth = _ScanWidth * 0.5;
                float innerEdge = _ScanRadius - halfWidth;
                float outerEdge = _ScanRadius + halfWidth;

                float ringOuter = smoothstep(innerEdge - _ScanFalloff, innerEdge, dist);
                float ringInner = 1.0 - smoothstep(outerEdge, outerEdge + _ScanFalloff, dist);
                float ringMask = ringOuter * ringInner;

                #if UNITY_REVERSED_Z
                    float skyMask = step(0.0001, depth);
                #else
                    float skyMask = step(depth, 0.9999);
                #endif

                float3 dirToPixel = normalize(worldPos - _ScanOrigin);
                float cosAngle = dot(dirToPixel, _ScanDirection);
                float cosThreshold = cos(radians(_ScanAngle));
                float angleMask = smoothstep(cosThreshold - _ScanAngleSoftness, cosThreshold, cosAngle);

                float3 ringColor = _ScanColor.rgb * ringMask * angleMask * _ScanIntensity * skyMask;

                return float4(sceneColor.rgb + ringColor, sceneColor.a);
            }
            ENDHLSL
        }
    }
}