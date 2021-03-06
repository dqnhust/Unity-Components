Shader "Custom/BasicSkyFactor"{
    Properties {
        _Color ("Color Tint", Color) = (1.0,1.0,1.0,1.0)
        _AmbientColor ("Ambient Color", Color) = (1,1,1,1.0)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1.0)
        _MainTex ("Diffuse Texture", 2D) = "white" {}

        [Header(Factor)]
        _DiffuseStrength ("Diffuse Factor", Range(0, 1)) = 0.5
        _AmbientStrength ("Ambient Factor", Range(0, 1)) = 0.5
        _ShadowStrength ("Shadow Factor", Range(0, 1)) = 1	
        _SkyFactor("Sky Factor", Range(0, 1)) = 0
    }
    SubShader {
        Pass {
            Tags {"LightMode" = "ForwardBase"}
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
            uniform half _SkyFactor;            

            struct vertexInput{
                half4 vertex : POSITION;
                half3 normal : NORMAL;
                half4 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct vertexOutput{
                half4 pos : SV_POSITION;
                half4 tex : TEXCOORD0;
                half3 worldRefl : TEXCOORD1;
                SHADOW_COORDS(2)
                half4 diffuse : COLOR;
            };
            
            vertexOutput vert(vertexInput v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                vertexOutput o;
                
                half3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                half3 normalDir = normalize( mul( half4( v.normal, 0.0 ), unity_WorldToObject ).xyz );
                o.pos = UnityObjectToClipPos(v.vertex);
                o.tex = v.texcoord;

                half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                half nl = saturate(dot(normalDir, lightDirection));
                o.diffuse = _LightColor0 * nl * _DiffuseStrength;

                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldRefl = reflect(-worldViewDir, worldNormal);

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

                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.worldRefl);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
                color.rgb = lerp(color.rgb, skyColor.rgb, _SkyFactor);
                // color.rgb *= skyColor.rgb*_SkyFactor;

                return color;
            }
            
            ENDCG
            
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

    }
    //Fallback "Specular"
}