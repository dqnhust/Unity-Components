Shader "Custom/SimpleGrabPassBlur" {
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", COLOR) = (1, 1, 1, 1)
        _Radius("Radius", Range(1, 1024)) = 1
    }
    Category
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        
        SubShader
        {
            GrabPass
            {
                Tags{ "LightMode" = "Always" }
            }
            Pass
            {
                Tags{ "LightMode" = "Always" }
                Blend SrcAlpha OneMinusSrcAlpha
                
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"

                uniform float4 _Color;

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord: TEXCOORD0;
                };
                struct v2f
                {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
                    float2 uv:TEXCOORD1;
                };
                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #if UNITY_UV_STARTS_AT_TOP
                        float scale = -1.0;
                    #else
                        float scale = 1.0;
                    #endif
                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;
                    o.uv = v.texcoord;
                    return o;
                }
                sampler2D _MainTex;
                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;
                float _Radius;
                half4 frag(v2f i) : COLOR
                {
                    half4 sum = half4(0,0,0,0);
                    #define GRABXYPIXEL(kernelx, kernely) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)))
                    sum += GRABXYPIXEL(0.0, 0.0);
                    int measurments = 1;
                    float currentStep = 128;
                    for (float range = currentStep; range <= _Radius; range += currentStep)
                    {
                        sum += GRABXYPIXEL(range, range);
                        sum += GRABXYPIXEL(range, -range);
                        sum += GRABXYPIXEL(-range, range);
                        sum += GRABXYPIXEL(-range, -range);
                        measurments += 4;
                    }
                    float4 color = sum / measurments;
                    float4 texColor = tex2D(_MainTex, i.uv);
                    texColor.rgb = lerp(1, texColor.rgb, texColor.a);
                    color.rgb *= texColor.rgb;
                    
                    return color;
                    // return sum / measurments * i.color;
                }
                ENDCG
            }
        }
    }
}