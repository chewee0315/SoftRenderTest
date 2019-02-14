Shader "SoftRender/Test/2D"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "black" {}
	}
	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			//"IgnoreProjector"="False"
			"RenderType"="Transparent"
		}

		Cull Off
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4	vertex	: POSITION;
				float2	uv		: TEXCOORD0;
				float2	uv2		: TEXCOORD1;	// u:pers でのデプス.
			};

			struct v2f
			{
				float4	vertex	: SV_POSITION;

				float2	uv		: TEXCOORD0;
				float4	color	: TEXCOORD1;
			};

			sampler2D	_MainTex;
			//float4x4	_MvpMatrix;
			float4x4	_c_view_matrix;
			float4x4	_c_proj_matrix;
	
			v2f		vert(appdata v)
			{
				v2f	o = (v2f)0;

				o.vertex = v.vertex;

				//o.vertex = UnityObjectToClipPos(o.vertex);
				//o.vertex = mul(UNITY_MATRIX_M, o.vertex);
				//o.vertex = mul(_c_view_matrix, o.vertex);
				//o.vertex = mul(_c_proj_matrix, o.vertex);

				// 3D の描画との前後関係が破たんしないよう、Z値が Perspective と
				// 同じになるようにする
				//
				o.vertex.z = v.uv2.x;

				o.uv = v.uv;

				return(o);
			}
			
			fixed4	frag(v2f i) : SV_Target
			{
				fixed4	col = tex2D(_MainTex, i.uv);

				//col = fixed4(1.0f, 1.0f, 1.0f, 1.0f);

				return(col);
			}
			ENDCG
		}
	}
}
