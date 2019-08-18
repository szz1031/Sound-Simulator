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
			};

			struct v2f
			{
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(0,0,0,0);
			}
			ENDCG
		}
	}
}
