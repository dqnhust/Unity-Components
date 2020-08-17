Shader "Custom/Water"
{
    Properties
    {
        _Color ("Color", COLOR) = (1, 1, 1, 1)
        _ColorUp ("Color Up", COLOR) = (1, 1, 1, 1)
        _Texture ("Texture", 2D) = "white" {}
        _NoiseTexture ("Noise Texture", 2D) = "white" {}
        _BoostUp ("BoostUp", Range(1, 10)) = 0
        _LimitUp ("Limit Up", Range(0, 1)) = 1
        _Speed ("Speed", float) = 0
        _Strength("Strength", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;
            sampler2D _Texture;
            float4 _Texture_ST;
            float4 _Color;
            float4 _ColorUp;
            float _BoostUp;
            float _Speed;
            float _Strength;
            float _LimitUp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float PingPong(float v, float limit) {
                while (v < 0 || v > limit) {
                    if (v < 0) {
                        v *= -1;
                    }
                    if (v > limit) {
                        v = limit - (v - limit);
                    }
                }
                return v;
            }

            float GetNoiseFloat(float2 uv) {
                return tex2D(_NoiseTexture, uv * _NoiseTexture_ST.xy + _NoiseTexture_ST.zw);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float noiseFloat = GetNoiseFloat(i.uv);
                float2 movedUv = i.uv;
                movedUv.x += sin(_Time.x * _Speed)*_Strength;
                movedUv.y += cos(_Time.x * _Speed)*_Strength;
                movedUv.x = PingPong(movedUv.x, 1);
                movedUv.y = PingPong(movedUv.y, 1);
                movedUv = lerp(i.uv, movedUv, noiseFloat);
                float4 heightColor = tex2D(_Texture, movedUv * _Texture_ST.xy + _Texture_ST.zw);
                float heightValue = (heightColor.r + heightColor.g + heightColor.b)/3;
                heightValue = pow(heightValue, _BoostUp * noiseFloat);
                heightValue = clamp(heightValue, 0, _LimitUp);
                float4 color = lerp(_Color, _ColorUp, heightValue);
                return color;
            }
            ENDCG
        }
    }
}
