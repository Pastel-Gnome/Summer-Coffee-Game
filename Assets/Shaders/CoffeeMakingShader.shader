Shader "custom/CoffeeMakingShader"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        
        [Header(Coffee Properties)]
        _FillAmount ("Fill Amount", Range(0,1)) = 0.0
        _CoffeeColor ("Tint", Color) = (1,1,1,1)
        
        [Header(Milk Properties)]
        [Toggle]_AddMilk ("Add Milk", Float) = 0.0  // Toggle for milk addition: 0 = No milk, 1 = Add milk
        _MilkColor ("MilkColor", Color) = ( 0.9, 0.76, 0.55, 1.0)
        _Blend ("Blend", Range(0,1)) = 0.0

        [Header(Foam Properties)]
        [Toggle]_AddFoam ("Add Foam", Float) = 0.0  // Toggle for Foam addition: 0 = No Foam, 1 = Add Foam
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _FoamLineHeight ("Foam Line Width", Range(0,0.1)) = 0.0

        [Header(Inner Glow Properties)]
        _InnerGlowColor ("Rim Color", Color) = (1,1,1,1)
        _InnerGlowPower ("Rim Power", Range(0,10)) = 0.0
        
        [Header(Wobble Behaviour Properties)]
        _WaveStrength ("Wave Strength", Float) = 2.0
        _WaveFrequency ("Wave Frequency", Float) = 180.0
    }

    SubShader
    {
        Tags {"Queue"="Geometry"  "DisableBatching" = "True" }
  
        Pass
        {
            Zwrite On
            Cull Off
            AlphaToMask On

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;    
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 viewDir : COLOR;
                float3 normal : COLOR2;        
                float fillEdge : TEXCOORD2;
            };
 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FillAmount;
            float4 _FoamColor, _InnerGlowColor, _CoffeeColor,_MilkColor;
            float _FoamLineHeight, _InnerGlowPower, _Blend;
            float _WaveStrength, _WaveFrequency;
            float _AddMilk, _AddFoam;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);            
                
                // Get world position of the vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);   
                
                float fillLevel = _FillAmount/2; 
                
                // Apply wobbling effect
                float wave = sin(10.0 * worldPos.y + 10.0 * worldPos.x + _WaveFrequency * _Time) * (_WaveStrength / 1000.0);
                wave += sin(20.0 * -worldPos.y + 20.0 * worldPos.x + _WaveFrequency * 0.5 * _Time) * (_WaveStrength / 2000.0);
                wave += sin(15.0 * -worldPos.y + 15.0 * -worldPos.x + _WaveFrequency * 0.6 * _Time) * (_WaveStrength / 769.2);
                wave += sin(3.0 * -worldPos.y + 3.0 * -worldPos.x + _WaveFrequency * 0.3 * _Time) * (_WaveStrength / 100.0);

                // Adjust fill edge with wobble and foam height
                o.fillEdge = worldPos.y + wave - fillLevel;

                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.normal = v.normal;
                return o;
            }
           
            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                if (_FillAmount > 0.0) {
                    
                    // Sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * _CoffeeColor;
                    
                    // Apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);
                
                    // Rim light
                    float dotProduct = 1 - pow(dot(i.normal, i.viewDir), _InnerGlowPower);
                    float4 RimResult = smoothstep(0.5, 1.0, dotProduct);
                    RimResult *= _InnerGlowColor;
                    if(_AddFoam == 0){
                        _FoamLineHeight = 0;
                    }   
                    // Foam edge
                    float foam = (step(i.fillEdge, 0.0) - step(i.fillEdge, -_FoamLineHeight));
                    float4 foamColored = foam * _FoamColor;
                
                    // Rest of the liquid
                    float result = step(i.fillEdge, 0.0) - foam;
                    float4 resultColored = result * col;
                
                    // Combine foam and liquid
                    float4 finalResult = resultColored + foamColored;                
                    finalResult.rgb += RimResult;
                   
                    if(_AddMilk == 1){
                       finalResult.rgb = lerp(finalResult.rgb, _MilkColor.rgb, _Blend);
                    }       
                    // Color of backfaces / toptaksmanager

                    float4 topColor = _CoffeeColor * (foam + result);
                    if(_AddFoam && _AddMilk){
                        topColor = lerp(_FoamColor, _MilkColor, _Blend) * (foam + result);     
                    }else if(_AddMilk == 1)
                    {
                        topColor = _MilkColor  * (foam + result);
                    }else if(_AddFoam == 1)
                    {
                        topColor = _FoamColor  * (foam + result);
                    }
                    // VFACE returns positive for front facing, negative for backfacing
                    return facing > 0 ? finalResult : topColor;
                   
                }
                return fixed4(1,1,1,0);
            }
            ENDCG
        }
    }
}
