Shader "Hovl/Particles/Distortion_Optimized"
{
    Properties
    {
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Distortionpower("Distortion Power", Float) = 0.05
        [Range(0.01, 3.0)] _InvFade("Soft Particles Factor", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest LEqual
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _NormalMap;
            float4 _NormalMap_ST;
            float _Distortionpower;
            float _InvFade;

            sampler2D _CameraOpaqueTexture;
            float4 _CameraOpaqueTexture_TexelSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 grabUV : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _NormalMap);
                o.color = v.color;

                // ������Ļ�ռ� UV
                float4 screenPos = o.vertex;
                screenPos /= screenPos.w;
                screenPos.xy = screenPos.xy * 0.5 + 0.5;
                o.grabUV = screenPos.xy;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ����������ͼ
                fixed3 normal = UnpackNormal(tex2D(_NormalMap, i.texcoord));

                // ����ƫ����
                float2 offset = normal.xy * _Distortionpower;

                // �����µ� UV ����
                float2 uv = i.grabUV + offset;

                // ������Ļ����
                fixed4 col = tex2D(_CameraOpaqueTexture, uv);

                // Ӧ��͸����
                col.a = i.color.a;

                // Ӧ����Ч
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
