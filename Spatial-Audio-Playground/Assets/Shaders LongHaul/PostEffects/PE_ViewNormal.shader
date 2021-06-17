Shader "Hidden/PostEffect/PE_ViewNormal"
{
	Properties
	{
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

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			sampler2D _CameraDepthNormalsTexture;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 normal = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture,i.uv));
				return fixed4(normal*.5+.5,1);
			}
			ENDCG
		}
	}
}
