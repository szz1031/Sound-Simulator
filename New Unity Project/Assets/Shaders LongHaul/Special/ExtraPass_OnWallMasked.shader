Shader "Hidden/ExtraPass/OnWallMasked"
{
	Properties
	{
		_MaskColor("Masked Color",Color) = (1,1,1,.5)
	}
		SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"
		float4 _MaskColor;
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
				return _MaskColor;
			}
			ENDCG
		}

		Pass
		{
			Name "Rim"
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
				float rim : TEXCOORD0;
			};
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normal =normalize(v.normal);
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				o.rim = (1 - pow(dot(normal, viewDir), 2));
				return o;
			}
			fixed4 frag(v2f i) :SV_TARGET
			{
				return i.rim *_MaskColor;
			}
			ENDCG
		}

		Pass
		{
			Name "Outline_First"
			Blend SrcAlpha One
			ZWrite Off
			Cull Back
			ZTest Greater
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			struct appdata
			{
				float4 vertex:POSITION;
				float3 normal:NORMAL;
			};
			struct v2f {
				float4 pos:SV_POSITION;
				float3 viewDir:TEXCOORD1;
				float3 normal:TEXCOORD0;
			};

			v2f vert(appdata v) {
				v2f o;
				float3 dir = normalize(v.vertex.xyz);
				float3 dir2 = normalize(v.normal);
				dir = dir * sign(dot(dir, dir2));
				
				o.pos = UnityObjectToClipPos(v.vertex + dir * .05f);
				o.normal = v.normal;
				o.viewDir = ObjSpaceViewDir(v.vertex);
				return o;
			}
			float4 frag(v2f i) :COLOR
			{
				return _MaskColor;
			}
			ENDCG
		}
				Pass
			{
				Name "Outline_Second"
				Blend Off
				ZWrite Off
				Cull Back
				ZTest Greater
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag			
				struct appdata
				{
					float4 vertex:POSITION;
				};
				struct v2f {
					float4 pos:SV_POSITION;
				};

				v2f vert(appdata v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex );
					return o;
				}
				float4 frag(v2f i) :COLOR
				{
					return fixed4(0,0,0,1);
				}
				ENDCG
			}
	}
}
