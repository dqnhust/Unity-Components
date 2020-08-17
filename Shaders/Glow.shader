// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Glow"{
    Properties {
        _Color ("Color Tint", Color) = (1.0,1.0,1.0,1.0)
        _AmbientColor ("Ambient Color", Color) = (1,1,1,1.0)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1.0)
        _MainTex ("Diffuse Texture", 2D) = "white" {}

        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowSize ("Glow Size", float) = 1
        _GlowAlphaPow ("Glow Alpha Pow", float) = 1

        [Header(Factor)]
        _DiffuseStrength ("Diffuse Factor", Range(0, 1)) = 0.5
        _AmbientStrength ("Ambient Factor", Range(0, 1)) = 0.5
        _ShadowStrength ("Shadow Factor", Range(0, 1)) = 1	
    }
    SubShader {
        Tags {
            "LightMode" = "ForwardBase"
            "Queue" = "Transparent"
        }
        Pass {
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma exclude_renderers flash
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight	
            #pragma multi_compile_instancing		

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"
            
            uniform sampler2D _MainTex;
            uniform half4 _MainTex_ST;
            uniform half4 _Color;
            uniform half _DiffuseStrength;
            uniform half4 _AmbientColor;
            uniform half4 _ShadowColor;
            uniform half _ShadowStrength;
            uniform half _AmbientStrength;            

            struct vertexInput{
                half4 vertex : POSITION;
                half3 normal : NORMAL;
                half4 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct vertexOutput{
                half4 pos : SV_POSITION;
                half4 tex : TEXCOORD0;
                half3 worldPos : TEXCOORD1;                    
                SHADOW_COORDS(2)
                half4 diffuse : COLOR;
            };
            
            vertexOutput vert(vertexInput v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                vertexOutput o;
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                half3 normalDir = normalize( mul( half4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
                o.pos = UnityObjectToClipPos(v.vertex);
                o.tex = v.texcoord;

                half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                half nl = saturate(dot(normalDir, lightDirection));
                o.diffuse = _LightColor0 * nl * _DiffuseStrength;

                TRANSFER_SHADOW(o);

                return o;
            }
            
            half4 frag(vertexOutput i) : COLOR
            {			

                half3 ambient = _AmbientColor.rgb * _AmbientStrength;
                half shadow = SHADOW_ATTENUATION(i);

                half4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);

                shadow = lerp(shadow, 1, 1 - _ShadowStrength);
                half3 shadowedDiffuse = lerp(_ShadowColor, i.diffuse.rgb, shadow);
                half3 light = shadowedDiffuse + ambient;

                half4 color;
                color.rgb = tex.rgb * _Color * light;
                color.rgb = lerp(color.rgb, shadowedDiffuse, 1 - shadow);
                color.a = tex.a;

                return color;
            }
            
            ENDCG
            
        }


        Pass {
            Name "Glow"
            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag


            uniform float _GlowSize;
            uniform float4 _GlowColor;
            uniform float _GlowAlphaPow;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f {
                float4 pos : POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v) {

                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);
                o.pos.xy += offset * _GlowSize;
                o.worldNormal = mul( unity_ObjectToWorld, float4( v.normal, 0.0 ) ).xyz;;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            half4 frag(v2f i) : COLOR {
                float3 camForward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
                float d = dot(i.worldNormal, camForward);
                if (d > 0) {
                    d = pow(d, _GlowAlphaPow);
                }
                return float4(_GlowColor.rgb, abs(d));
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}