Shader "Hidden/ExtraPass/OnWallMasked"
{
	Properties
	{
	}
	SubShader
	{
		CGINCLUDE
#include "UnityCG.cginc"
		ENDCG
		Pass
		{
			Name "SimpleColor"
			Blend SrcAlpha One
			ZWrite Off
			Cull Back
			ZTest Greater
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct v2f
			{
				float4 pos:SV_POSITION;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			fixed4 frag(v2f i) :SV_TARGET
			{
				return fixed4(1,1,1,.5);
			}
			ENDCG
		}

		Pass
		{
			Name "RimLight"
			Blend SrcAlpha One
			ZWrite Off
			Cull Off
			ZTest Greater
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct v2f
			{
				float4 pos:SV_POSITION;
				float3 normal:NORMAL;
				float3 viewDir:TEXCOORD1;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal =normalize( v.normal);
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
				return o;
			}
			fixed4 frag(v2f i) :SV_TARGET
			{
				float rim = dot(i.normal,i.viewDir);
				
				return (1-pow(rim,2))*fixed4(1,1,1,.5f);
			}
			ENDCG
		}


	}
}
