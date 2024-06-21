Shader "Custom/Barrier"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 0.5) // 알파 값이 0.5로 설정되어 반투명
        _EffectSpeed ("Effect Speed", Float) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 200

        Pass
        {
            Cull Off

            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 설정
            ZWrite Off // 깊이 쓰기 비활성화

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _EffectSpeed;
            float4 _RimColor;
            float _RimPower;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += (_Time.y * _EffectSpeed);
                half4 texColor = tex2D(_MainTex, uv) * _Color;
                return texColor;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}