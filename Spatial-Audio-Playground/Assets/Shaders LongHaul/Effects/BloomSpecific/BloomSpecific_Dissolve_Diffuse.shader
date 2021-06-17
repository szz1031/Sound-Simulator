Shader "Game/Effect/BloomSpecific/Dissolve_Diffuse"
{
	Properties
	{
		_MainTex("Main Tex",2D) = "white"{}
		_Color("Color",Color) = (1,1,1,1)


		_SubTex1("Dissolve Map",2D) = "white"{}
		_Amount1("_Dissolve Progress",Range(0,1)) = 1
		_Amount2("_Dissolve Width",float) = .1
	}
	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "Lighting.cginc"

		sampler2D _SubTex1;
		float4 _SubTex1_ST;
		float _Amount1;
		float _Amount2;
		float4 _Color;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		ENDCG

		Pass		//Base Pass
		{
			Tags{ "LightMode" = "ForwardBase" "Queue" = "Transparent"}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
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
				float4 uv:TEXCOORD0;
				float3 worldPos:TEXCOORD1;
				float diffuse:TEXCOORD2;
				SHADOW_COORDS(3)
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.uv.xy = TRANSFORM_TEX( v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _SubTex1);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos =mul(unity_ObjectToWorld,v.vertex);
				fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject)); //法线方向n
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));
				o.diffuse = saturate(dot(worldLightDir,worldNormal));
				TRANSFER_SHADOW(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed dissolve = tex2D(_SubTex1,i.uv.zw).r - _Amount1-_Amount2;
				clip(dissolve);

				float3 albedo = tex2D(_MainTex,i.uv.xy)* _Color;
				UNITY_LIGHT_ATTENUATION(atten, i,i.worldPos)
				fixed3 ambient = albedo*UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 diffuse = albedo* _LightColor0.rgb*i.diffuse*atten;
				return fixed4(ambient+diffuse,1);
			}
			ENDCG
		}

			Pass
			{
				Name "ForwardAdd"
				Tags{"LightMode" = "ForwardAdd"}
				Blend One One
				CGPROGRAM
				#pragma multi_compile_fwdadd
				#pragma vertex vertAdd
				#pragma fragment fragAdd

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
					float3 worldPos:TEXCOORD1;
					float diffuse : TEXCOORD2;
				};

				v2f vertAdd(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					o.uv = TRANSFORM_TEX(v.uv,_MainTex);

					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject)); //法线方向n
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(o.worldPos));
					o.diffuse = saturate(dot(worldLightDir, worldNormal));

					return o;
				}

				fixed4 fragAdd(v2f i) :SV_TARGET
				{
					fixed3 albedo = tex2D(_MainTex, i.uv);

					fixed3 diffuse = i.diffuse*_LightColor0.rgb;

					UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

					return fixed4(diffuse * atten,1);
				}
					ENDCG
			}

		Pass
		{
			Cull Off
			Tags{"LightMode" = "ShadowCaster"}
			CGPROGRAM
			#pragma vertex vertshadow
			#pragma fragment fragshadow

			struct v2fs
			{
				V2F_SHADOW_CASTER;
				float2 uv:TEXCOORD0;
			};
			v2fs vertshadow(appdata_base v)
			{
				v2fs o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _SubTex1);
				return o;
			}

			fixed4 fragshadow(v2fs i) :SV_TARGET
			{
				fixed dissolve = tex2D(_SubTex1,i.uv).r - _Amount1-_Amount2;
				clip(dissolve);
				SHADOW_CASTER_FRAGMENT(i);
			}
			ENDCG
		}


	}
}
