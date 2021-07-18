Shader "Game/Effect/BloomSpecific/Bloom_DissolveEdge"
{
	Properties
	{
		_SubTex1("Dissolve Map",2D) = "white"{}
		_Amount1("_Dissolve Progress",Range(0,1)) = 1
		_Amount2("_Dissolve Width",float) = .1
		_Color1("_Dissolve Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		Name "MAIN"
		Tags { "RenderType"="BloomDissolveEdge" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
			CGINCLUDE
		#include "UnityCG.cginc"

			sampler2D _SubTex1;
			float4 _SubTex1_ST;
			float _Amount1;
			float _Amount2;
			float4 _Color1;
			ENDCG
		Pass		//Base Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float2 uv:TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv:TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _SubTex1);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed dissolve = tex2D(_SubTex1,i.uv).r - _Amount1;
				clip(dissolve);
				if (dissolve < _Amount2)
					return _Color1;
				clip(-1);
				return _Color1;
			}
			ENDCG
		}

	}
}
