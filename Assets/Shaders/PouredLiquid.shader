Shader "custom/PouredLiquid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} 
        _Color ("Color", Color) = (1,1,1,1) 
        _Speed("Speed", float) = 0.0 
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent" 
            "PreviewType" = "Plane" 
            "CanUseSpriteAtlas" = "False" 
        }

        Blend SrcAlpha OneMinusSrcAlpha 
        Cull Off 
        Lighting Off 
        ZWrite Off 

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc" 

            sampler2D _MainTex;
            float4 _Color;
            float _Speed;

            struct appdata_t
            {
                float4 vertex : POSITION; 
                float2 texcoord : TEXCOORD0; 
                float4 color : COLOR; 
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; 
                float4 vertex : SV_POSITION; 
                float4 color : COLOR; 
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); 
                o.uv = v.texcoord; 
                o.color = v.color * _Color; 
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pannedUV = i.uv;
                pannedUV.x -= _Time.y * _Speed;

                fixed4 texColor = tex2D(_MainTex, pannedUV);

                fixed4 finalColor = texColor * i.color;

                return finalColor;
            }

            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }
    }

    Fallback "Transparent/VertexLit" 
}
