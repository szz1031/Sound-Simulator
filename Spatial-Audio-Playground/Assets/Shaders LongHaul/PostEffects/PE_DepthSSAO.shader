Shader "Hidden/PostEffect/PE_DepthSSAO"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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

			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			sampler2D _CameraDepthTexture;
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				half2 uv_depth:TEXCOORD1;
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
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth));
				
				fixed3 col = tex2D(_MainTex, i.uv).rgb;



				return fixed4(linearDepth, linearDepth, linearDepth,1);
			}
			ENDCG
		}
	}
}
