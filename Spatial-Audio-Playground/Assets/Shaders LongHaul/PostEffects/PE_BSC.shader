Shader "Hidden/PostEffect/PE_BSC"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Brightness("Brightness",Range(0,3)) = 1
		_Saturation("Saturation", Range(0, 3)) = 1
		_Contrast("Contrast", Range(0, 3)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		CGINCLUDE
			fixed luminance(fixed3 color)
		{
		return 0.2125*color.r + 0.7154*color.g + 0.0721 + color.b;
		}
			ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			fixed _Brightness;
			fixed _Saturation;
			fixed _Contrast;

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
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 col = tex2D(_MainTex, i.uv).rgb*_Brightness;
				
			fixed lum = luminance(col);
			fixed3 lumCol = fixed3(lum, lum, lum);
			col = lerp(lumCol, col, _Saturation);
			
			fixed3 avgCol = fixed3(.5, .5, .5);
			col = lerp(avgCol, col, _Contrast);

			return fixed4( col,1);
			}
			ENDCG
		}
	}
}
