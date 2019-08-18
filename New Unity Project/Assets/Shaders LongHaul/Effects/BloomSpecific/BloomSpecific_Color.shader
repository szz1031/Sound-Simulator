Shader "Game/Effect/BloomSpecific/Color"
{
	Properties
	{
	    _MainTex("Texture", 2D) = "white" {}
	    _Color("_Color",Color)=(1,1,1,1)
	}

	SubShader
	{ 
		Tags {"RenderType" = "BloomColor" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Back Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		Blend SrcAlpha OneMinusSrcAlpha

		Cull Back
		Pass
		{
			name "MAIN"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float4 color    : COLOR;
				float2 uv:TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color    : TEXCOORD0;
				float2 uv:TEXCOORD1;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv)*_Color*i.color;
				return col;
			}
			ENDCG
		}
	}
}
