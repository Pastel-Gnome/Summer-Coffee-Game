Shader "custom/CoffeeMakingShader"
{
    Properties
    {
        _Progress ("Progress", Range(0.0, 1.0)) = 0.5
        _CoffeeColor ("CoffeeColor", Color) = (0.4, 0.2, 0.1, 1.0)
        _MilkColor ("MilkColor", Color) = ( 0.9, 0.76, 0.55, 1.0)
        [Toggle]_AddMilk ("Add Milk", Float) = 0.0  // Toggle for milk addition: 0 = No milk, 1 = Add milk
        _WaveStrength ("WaveStrength", Float) = 2.0
        _WaveFrequency ("WaveFrequency", Float) = 180.0
        _WaterTransparency ("WaterTransparency", Float) = 1.0
        _WaterAngle ("WaterAngle", Float) = 4.0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Geometry"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float _Progress;
            fixed4 _CoffeeColor;     
            fixed4 _MilkColor;
            float _AddMilk;
            float _WaveStrength;
            float _WaveFrequency;
            float _WaterTransparency;
            float _WaterAngle;

            fixed4 drawLiquid(fixed4 coffee_color, fixed4 milk_color, float add_milk, sampler2D color, float transparency, float height, float angle, float wave_strength, float wave_frequency, fixed2 uv)
            {
                float iTime = _Time;
                angle *= uv.y / height + angle / 1.5;
                wave_strength /= 1000.0;
                float wave = sin(10.0 * uv.y + 10.0 * uv.x + wave_frequency * iTime) * wave_strength;
                wave += sin(20.0 * -uv.y + 20.0 * uv.x + wave_frequency * 1.0 * iTime) * wave_strength * 0.5;
                wave += sin(15.0 * -uv.y + 15.0 * -uv.x + wave_frequency * 0.6 * iTime) * wave_strength * 1.3;
                wave += sin(3.0 * -uv.y + 3.0 * -uv.x + wave_frequency * 0.3 * iTime) * wave_strength * 10.0;

                if (uv.y - wave <= height)
                {
                    // Determine the liquid color based on whether milk is added
                    fixed4 liquidColor = lerp(coffee_color, milk_color, add_milk*0.5);
                    return lerp(
                        lerp(
                            tex2D(color, fixed2(uv.x, ((1.0 + angle) * (height + wave) - angle * uv.y + wave))),
                            liquidColor,
                            0.6 - (0.3 - (0.3 * uv.y / height))),
                        tex2D(color, fixed2(uv.x + wave, uv.y - wave)),
                        transparency - (transparency * uv.y / height)
                    );
                }
                else
                {
                    return fixed4(0, 0, 0, 0);
                }
            }

            fixed4 frag (v2f i) : COLOR
            {
                fixed2 uv = i.texcoord;
                float WATER_HEIGHT = _Progress;
                float4 COFFEE_COLOR = _CoffeeColor;
                float4 MILK_COLOR = _MilkColor;
                float ADD_MILK = _AddMilk;
                float WAVE_STRENGTH = _WaveStrength;
                float WAVE_FREQUENCY = _WaveFrequency;
                float WATER_TRANSPARENCY = _WaterTransparency;
                float WATER_ANGLE = _WaterAngle;

                fixed4 fragColor = drawLiquid(COFFEE_COLOR, MILK_COLOR, ADD_MILK, _MainTex, WATER_TRANSPARENCY, WATER_HEIGHT, WATER_ANGLE, WAVE_STRENGTH, WAVE_FREQUENCY, uv);
                return fragColor;
            }
            ENDCG
        }
    }
}
