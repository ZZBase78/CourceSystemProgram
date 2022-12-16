// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MyShaders/AtmosphereShader"
{
    Properties{
        _Tex1("Texture1", 2D) = "white" {} // текстура1
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Glow("Intensity", Range(0, 3)) = 1
		_Height("Height", Range(0,1)) = 0.5 // сила изгиба
    }
		//SubShader
		//{
		//	Tags{ "RenderType" = "Opaque" } // тег, означающий, что шейдер непрозрачный
		//		LOD 100 // минимальный уровень детализации
		//		Pass
		//	{
		//	CGPROGRAM
		//	#pragma vertex vert // директива для обработки вершин
		//	#pragma fragment frag // директива для обработки фрагментов
		//	#include "UnityCG.cginc" // библиотека с полезными функциями
		//	sampler2D _Tex1; // текстура1
		//	float4 _Tex1_ST;
		//	float4 _Color; // цвет, которым будет окрашиваться изображение

		//	// структура, которая помогает преобразовать данные вершины в данные фрагмента
		//	struct v2f
		//	{
		//	float2 uv : TEXCOORD0; // UV-координаты вершины
		//	float4 vertex : SV_POSITION; // координаты вершины
		//	};
		//	//здесь происходит обработка вершин
		//	v2f vert(appdata_full v)
		//	{
		//	v2f result;
		//	float pi = 3.14159265358979323846264338327;
		//	//v.vertex.xyz += v.normal * _Height * v.texcoord.x * v.texcoord.x;
		//	//v.vertex.xyz += v.normal * _Height * (1 - _SinTime.z);
		//	//v.vertex.xyz += sin(v.normal * _Height * v.texcoord.x);
		//	//v.vertex.xyz += v.normal * (_Height * sin(v.texcoord.x * pi) - _Height);
		//	//v.vertex.xyz += v.normal * v.texcoord.x;

		//	result.vertex = UnityObjectToClipPos(v.vertex);
		//	result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
		//	return result;
		//	}
		//	//здесь происходит обработка пикселей, цвет пикселей умножается на цвет материала
		//	fixed4 frag(v2f i) : SV_Target
		//	{
		//	fixed4 color;
		//	color = tex2D(_Tex1, i.uv);
		//	color = color * _Color;
		//	return color;
		//	}
		//	ENDCG
		//	}
		//}


			SubShader{
			 //Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			 //LOD 100
			 //Cull Off
			 //ZWrite On
			 Blend SrcAlpha OneMinusSrcAlpha

			 Pass {
				 CGPROGRAM
					 #pragma vertex vert
					 #pragma fragment frag

					 sampler2D _MainTex;
					 half4 _MainTex_ST;
					 fixed4 _Color;
					 half _Glow;
					 float _Height;

					 struct vertIn {
						 float3 pos : POSITION;
						 half2 tex : TEXCOORD0;
						 float3 normal : NORMAL;
					 };

					 struct v2f {
						 float4 pos : SV_POSITION;
						 half2 tex : TEXCOORD0;
					 };

					 v2f vert(vertIn v) {
						 v2f o;
						 //v.pos.x += _Time.y;
						 v.pos += v.normal * _Height;
						 o.pos = UnityObjectToClipPos(v.pos);
						 o.tex = v.tex * _MainTex_ST.xy + _MainTex_ST.zw;
						 o.tex += _Time.x / 2;
						 return o;
					 }

					 fixed4 frag(v2f f) : SV_Target {
						 fixed4 col = tex2D(_MainTex, f.tex);
						 col *= _Color;
						 col *= _Glow;
						 return col;
					 }
				 ENDCG
			 }
			}
}