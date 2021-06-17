Shader "Hidden/PostEffect/PE_BloomSpecific"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RenderTex("Render",2D) = "white"{}
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _RenderTex;
	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};
	ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			name "Minus"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col =tex2D(_RenderTex,i.uv)- tex2D(_MainTex,i.uv) ;
				return col;
			}
			ENDCG
		}

		Pass
		{		
			name "Mix"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv) +tex2D(_RenderTex,i.uv);
				return col;
			}
			ENDCG
		}
	}
}
