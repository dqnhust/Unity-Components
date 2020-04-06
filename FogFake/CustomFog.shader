// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/Fog" {
Properties {
    _Color ("Color", Color) = (1, 1, 1, 1)
    _MinDistanceFog("Min Distance Fog", FLOAT) = 0
    _MaxDistanceFog("Max Distance Fog", FLOAT) = 0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            uniform float4 _Color;
            uniform float _MinDistanceFog;
            uniform float _MaxDistanceFog;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _Color;
                float distance = length(_WorldSpaceCameraPos - i.worldPos);

                if (distance > _MinDistanceFog) {
                    col.a = clamp((distance-_MinDistanceFog)/(_MaxDistanceFog - _MinDistanceFog) ,0, 1);
                }
                return col;
            }
        ENDCG
    }
}

}
