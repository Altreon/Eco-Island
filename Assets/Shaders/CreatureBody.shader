// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CreatureBody" {
    Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)

        _Transparent ("Transparency", Float) = 1

        _Progression ("Progression", Float) = 0.000000

        _FaceTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	SubShader {
        Pass {
            //Tags { "RenderType"="Opaque" }
            //Tags {"Queue"="Transparent" "RenderType"="Transparent" }

            Blend SrcAlpha OneMinusSrcAlpha
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            LOD 200
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
            uniform float4 _MainColor;
            uniform float _Transparent;

            uniform float _Progression;

            uniform sampler2D _FaceTex;

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            float3x3 AngleAxis3x3(float angle, float3 axis)
            {
                float c, s;
                sincos(angle, s, c);

                float t = 1 - c;
                float x = axis.x;
                float y = axis.y;
                float z = axis.z;

                return float3x3(
                    t * x * x + c,      t * x * y - s * z,  t * x * z + s * y,
                    t * x * y + s * z,  t * y * y + c,      t * y * z - s * x,
                    t * x * z - s * y,  t * y * z + s * x,  t * z * z + c
                );
            }

            v2f vert(appdata_base v) {
                v2f o;
                v.vertex.x += sign(v.vertex.x) * sin(_Progression)/50;
				v.vertex.z += sign(v.vertex.z) * cos(_Progression)/50;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.normal = mul(AngleAxis3x3(90 * 0.0174533, float3(-1, 0, 0)), v.normal);
                return o;
            }

            float Epsilon = 1e-10;
 
            float3 RGBtoHCV(in float3 RGB)
            {
                // Based on work by Sam Hocevar and Emil Persson
                float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
                float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                return float3(H, C, Q.x);
            }

            float3 HUEtoRGB(in float H)
            {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
            }

            float3 HSVtoRGB(in float3 HSV)
            {
                float3 RGB = HUEtoRGB(HSV.x);
                return ((RGB - 1) * HSV.y + 1) * HSV.z;
            }

            float3 RGBtoHSV(in float3 RGB)
            {
                float3 HCV = RGBtoHCV(RGB);
                float S = HCV.y / (HCV.z + Epsilon);
                return float3(HCV.x, S, HCV.z);
            }

			half4 frag(v2f i) : COLOR {
                half4 m = tex2D(_MainTex, frac(i.normal * 0.5 + 0.5));
                float3 hsv = RGBtoHSV(m * _MainColor);
                hsv.x += m.r * 0.2 - 0.1;
                hsv.z = (m.r * m.r) * 3;
                half4 rgb = half4(HSVtoRGB(hsv), 1);
                half4 f = tex2D(_FaceTex, i.uv);
                half4 color = (f * f.a) + (rgb * (1 - f.a));
                color.a = _Transparent;
                return color;
            }

			ENDCG
		}
	} 
	FallBack "Legacy Shaders/Transparent/Diffuse"
}