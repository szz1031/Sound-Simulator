Shader "Hidden/PostEffect/PE_FogDepth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogDensity("Fog Density",Float) = 1
		_FogColor("Fog Color",Color) = (1,1,1,1)
		_FogStart("Fog Start",Float) = 0
		_FogEnd("Fog End",Float) = 1
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

			float4x4 _FrustumCornersRay;
			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			sampler2D _CameraDepthTexture;
			half _FogDensity;
			fixed4 _FogColor;
			float _FogStart;
			float _FogEnd;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				half2 uv_depth:TEXCOORD1;
				float4 interpolatedRay:TEXCOORD2;
			};

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.uv_depth = v.texcoord;
#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv_depth.y = 1 - o.uv_depth.y;
#endif
				int index = 0;
				if (o.uv.x > .5)
					index += 1;
				if (o.uv.y > .5)
					index += 2;
				o.interpolatedRay = _FrustumCornersRay[index];
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth));
				
				float3 worldPos = _WorldSpaceCameraPos+ i.interpolatedRay.xyz*linearDepth;
				float fogDensity = saturate( (_FogEnd - worldPos.y)*_FogDensity / (_FogEnd - _FogStart));
				fixed3 col = tex2D(_MainTex, i.uv).rgb;

				fogDensity = fogDensity == 1 ? _FogColor.a : fogDensity;

				col.rgb = lerp(col.rgb, _FogColor.rgb, fogDensity);

				return fixed4( col,1);
			}
			ENDCG
		}
	}
}
