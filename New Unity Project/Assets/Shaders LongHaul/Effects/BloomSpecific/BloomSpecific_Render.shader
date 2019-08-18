Shader "Hidden/PostEffect/PE_BloomSpecific_Render"
{
	Properties
	{

	}

	SubShader
	{
		Tags { "RenderType" = "BloomColor" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			float4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}

		SubShader
	{
		Tags{ "RenderType" = "BloomParticlesAdditive" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Particle/Additive/MAIN"
	}

		SubShader
	{
		Tags{ "RenderType" = "BloomParticlesAlphaBlend" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Particle/AlphaBlend/MAIN"
	}

		SubShader
	{
		Tags { "RenderType" = "BloomParticlesAdditiveNoiseFlow" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Particle/Additive_NoiseFlow/MAIN"
	}

		SubShader
	{
		Tags { "RenderType" = "BloomDissolveEdge" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Effect/BloomSpecific/Bloom_DissolveEdge/MAIN"
	}

		SubShader
	{
		Tags { "RenderType" = "BloomViewDirDraw" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Effect/BloomSpecific/Color_ViewDirDraw/MAIN"
	}
}