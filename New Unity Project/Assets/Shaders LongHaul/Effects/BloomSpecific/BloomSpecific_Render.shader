Shader "Hidden/PostEffect/PE_BloomSpecific_Render"
{
	SubShader
	{
		Tags { "RenderType" = "BloomColor" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
		UsePass "Game/Effect/BloomSpecific/Color/MAIN"
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

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue"="Geometry"}
		Pass
		{
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(0,0,0,1);
			}
			ENDCG
		}
	}

	//Additional SubShader 
	SubShader
	{
		Tags{"RenderType" = "BloomMask" "IgnoreProjectile" = "true" "Queue" = "Transparent"}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float2 uv:TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv:TEXCOORD0;
				float2 uvMask:TEXCOORD1;
			};

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SubTex1;
			float4 _SubTex1_ST;
			sampler2D _SubTex2;
			float4 _SubTex2_ST;
			float _Amount1;
			float _Amount2;
			float _Amount3;

			v2f vert(appdata v)
			{
				v2f o;
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _SubTex1);
				o.uvMask = TRANSFORM_TEX(v.uv, _SubTex2);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed dissolve = tex2D(_SubTex1,i.uv.zw).r - _Amount1 - _Amount2;
				clip(dissolve);

				float3 albedo = tex2D(_MainTex,i.uv.xy)* _Color;
				float4 colorMask = tex2D(_SubTex2, i.uvMask);
				if (colorMask.r == 1)
					return fixed4(albedo* abs(sin(_Time.y*_Amount3)), 1);
				else if (colorMask.r > 0)
					return fixed4(albedo, 1);

				return fixed4(0,0,0,1);
			}
			ENDCG
		}
	}
}
