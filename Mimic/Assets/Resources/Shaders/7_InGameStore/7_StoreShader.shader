Shader "Custom/2D/RoundedRectSimple" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius px", Float) = 150
        _Width ("Width px", Float) = 1900
        _Height ("Height px", Float) = 1000
    }
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Cull Off
        Lighting Off

        // Alpha blending.
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Width;
            float _Height;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float2 GetRadiusToPointVector(float2 pixel, float2 halfRes, float radius) {
                float2 firstQuadrant = abs(pixel);
                float2 radiusToPoint = firstQuadrant - (halfRes - radius);
                radiusToPoint = max(radiusToPoint, 0.0);
                return radiusToPoint;
            }

            // 날카로운 엣지를 생성합니다.
            float HardRounded(float2 pixel, float2 halfRes, float radius) {
                float2 v = GetRadiusToPointVector(pixel, halfRes, radius);
                float alpha = 1.0 - floor(length(v) / radius);
                return alpha;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uvInPixel = (i.uv - 0.5) * float2(_Width, _Height);
                float2 halfRes = float2(_Width, _Height) * 0.5;

                // 날카로운 엣지 사용
                float alpha = HardRounded(uvInPixel, halfRes, _Radius);

                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.a = alpha;
                return col;
            }
            ENDCG
        }
    }
}
