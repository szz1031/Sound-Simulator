Shader "Hidden/PostEffect/PE_EdgeDetectionDepth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	_ShowEdge("Show Edge",Range(0,1)) = 1
		_EdgeColor("Edge Color",Color) = (1,1,1,1)
		_SampleDistance("Sample Distance",Float) = 1
		_SensitivityDepth("Sensitivity Depth",Float) = 1
		_SensitivityNormals("Sensitivity Normals",Float) = 1
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

	sampler2D _CameraDepthNormalsTexture;
	sampler2D _MainTex;
	half4 _MainTex_TexelSize;
	fixed _ShowEdge;
	fixed4 _EdgeColor;
	fixed _SampleDistance;
	fixed _SensitivityDepth;
	fixed _SensitivityNormals;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv[5] : TEXCOORD0;
			};

			half CheckSame(half4 center, half4 sample)
			{
				half2 centerNormal = center.xy;
				half2 sampleNormal = sample.xy;
				half centerDepth = DecodeFloatRG(center.zw);
				half sampleDepth = DecodeFloatRG(sample.zw);

				half2 diffNormal = abs(centerNormal - sampleNormal)*_SensitivityNormals;
				half diffDepth = abs(centerDepth - sampleDepth)*_SensitivityDepth;

				int isSameNormal = (diffNormal.x + diffNormal.y) < .1;
				int isSameDepth = diffDepth < .1*centerDepth;

				return isSameNormal * isSameDepth ? 0 : 1;
			}

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				half2 uv = v.texcoord;
				o.uv[0] = uv;
#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					uv.y = 1 - uv.y;
#endif
				o.uv[1] = uv + _MainTex_TexelSize.xy*half2(1, 1)*_SampleDistance;
				o.uv[2] = uv + _MainTex_TexelSize.xy*half2(-1,-1)*_SampleDistance;
				o.uv[3] = uv + _MainTex_TexelSize.xy*half2(-1, 1)*_SampleDistance;
				o.uv[4] = uv + _MainTex_TexelSize.xy*half2(1, -1)*_SampleDistance;

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half edge = 1;
				edge *= CheckSame(tex2D(_CameraDepthNormalsTexture,i.uv[1]), tex2D(_CameraDepthNormalsTexture, i.uv[2]));
				edge *= CheckSame(tex2D(_CameraDepthNormalsTexture, i.uv[3]), tex2D(_CameraDepthNormalsTexture, i.uv[4]));

				fixed4 col =  tex2D(_MainTex, i.uv[0]);
				return lerp(col ,_EdgeColor,edge*_ShowEdge);
			}
			ENDCG
		}
	}
}
