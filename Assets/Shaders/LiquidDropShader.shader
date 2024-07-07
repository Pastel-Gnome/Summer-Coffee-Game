Shader "custom/LiquidDropShader"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = "white" {}
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        _InvertedFresnelColor("Inverted Fresnel Color", Color) = (1,1,1,1)
        _ScrollSpeed("Scroll Speed", Float) = 1.0
        _BaseWaterColor("Base Water Color", Color) = (0,0.5,1,1) // Default to a blueish color
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Opaque" 
            "Queue" = "Geometry"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;          
                float3 normal : NORMAL;     
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float3 worldPos : TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FlowCutoff;
            float _WobbleSpeed;
            float _ReverseDrop;
            float _FresnelIntensity;
            float _FresnelRamp;
            float _InvertedFresnelIntensity;
            float _InvertedFresnelRamp;
            float4 _FresnelColor, _InvertedFresnelColor;
            float _UVScaler;
            float _ScrollSpeed;
            float4 _BaseWaterColor; // New base water color property

            v2f vert(appdata v)
            {
                v2f o;

                // Vertex Displacement to cause mesh to "wobble"
                float originalPos = v.vertex.x;
                float newPos = -abs(originalPos * sin(_Time.y * _WobbleSpeed));

                // Only apply changes to mesh if it causes it to go "further away" than original mesh position
                if (newPos > originalPos) {
                    v.vertex.x = -abs((newPos) / 7 + originalPos);
                }

                // Transform vertex to clip space
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate world normal
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));

                // Sample textures
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Apply scrolling to the UVs along the y-axis
                o.uv.y += _Time.y * _ScrollSpeed;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Hides portions of the mesh for the drop in/drop out liquid animation
                // _ReverseDrop == 1 is used for Drop Animation, 0 to stop pouring.
                if (_ReverseDrop == 0) {
                    clip(-_FlowCutoff + i.uv.y);
                }
                
                // Base Water Color
                float4 baseColor = _BaseWaterColor;

                // Rim Color for liquid
                float fresnel = 1 - max(0, dot(i.normal, i.viewDir));
                fresnel = pow(fresnel, _FresnelRamp) * _FresnelIntensity;

                // Main Liquid Color effect with reverse fresnel
                float invertedFresnel = max(0, dot(i.normal, i.viewDir));
                invertedFresnel = pow(invertedFresnel, _InvertedFresnelRamp) * _InvertedFresnelIntensity;

                half4 textSample = tex2D(_MainTex, i.uv);

                // Applies color and combines both fresnel effects
                float4 fresnelColor = _FresnelColor * fresnel;
                float4 invertedFresnelColor = _InvertedFresnelColor * invertedFresnel;
                float4 color = fresnelColor + invertedFresnelColor + baseColor;

                // Makes wave texture color match rim liquid (fresnel) color
                if (textSample.r >= 0.3) {
                    textSample *= _FresnelColor;
                    color += textSample;
                }

                return color;
            }
            ENDCG
        }
    }
}
