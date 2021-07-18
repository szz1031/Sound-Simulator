Shader "Game/Effect/Dissolve_Diffuse_Full"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap("Normal Map",2D)="white"{}
		_DissolveAmount("Dissolve Amount",Range(0,1))=.5
		_DissolveMap("Dissolve Map",2D)="white"{}
		_DissolveWidth("Dissolve Width",Range(0,0.2)) = .1
		_DissolveStartColor("Dissolve Start Color",Color) = (1,0,0,1)
		_DissolveEndColor("Dissolve End Color",Color) = (0,1,0,1)
	}
	SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "Lighting.cginc"

		sampler2D _MainTex;
		half4 _MainTex_ST;
		sampler2D _BumpMap;
		fixed _DissolveAmount;
		sampler2D _DissolveMap;
		half4 _DissolveMap_ST;
		fixed _DissolveWidth;
		fixed4 _DissolveStartColor;
		fixed4 _DissolveEndColor;

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent:TANGENT;
			float3 normal:NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uvDissolve:TEXCOORD2;
			float3 tLightDir:TEXCOORD3;
			float3 worldPos:TEXCOORD4;
			SHADOW_COORDS(5)
		};		
		struct v2fa
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uvDissolve:TEXCOORD2;
			float3 tLightDir:TEXCOORD3;
			float3 worldPos:TEXCOORD4;
		};
		struct v2fs
		{
			V2F_SHADOW_CASTER;
			float2 uvDissolve:TEXCOORD1;
		};


		fixed4 frag(v2f i) : SV_Target
		{
			fixed dissolve = tex2D(_DissolveMap,i.uvDissolve).r;
			clip(dissolve - _DissolveAmount);

			float3 tangentLightDir = normalize(i.tLightDir);
			float3 tangentNormal = UnpackNormal(tex2D(_BumpMap,i.uv));

			fixed3 albedo = tex2D(_MainTex, i.uv);

			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;

			fixed3 diffuse = _LightColor0.rgb*albedo*max(0, dot(tangentLightDir, tangentNormal));

			fixed t = 1 - smoothstep(0, _DissolveWidth,dissolve-_DissolveAmount);
			fixed3 dissolveCol = lerp( _DissolveStartColor, _DissolveEndColor, t);

			UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

			return fixed4(lerp( ambient+diffuse*atten,dissolveCol,t*step(0.0001, _DissolveAmount)),1);
		}
		ENDCG
	

		Pass
		{
			Tags{"LightMode"="ForwardBase"}
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

				v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvDissolve = TRANSFORM_TEX(v.uv, _DissolveMap);

				TANGENT_SPACE_ROTATION;

				o.tLightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				TRANSFER_SHADOW(o);

				return o;
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

		v2fa vertAdd(appdata v)
		{
			v2fa o;
			o.pos = UnityObjectToClipPos(v.vertex);

			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uvDissolve = TRANSFORM_TEX(v.uv, _DissolveMap);

			TANGENT_SPACE_ROTATION;

			o.tLightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);

			return o;
		}

		fixed4 fragAdd(v2fa i) : SV_Target
		{
			fixed dissolve = tex2D(_DissolveMap,i.uvDissolve).r;
			clip(dissolve - _DissolveAmount);

			float3 tangentLightDir = normalize(i.tLightDir);
			float3 tangentNormal = UnpackNormal(tex2D(_BumpMap,i.uv));

			fixed3 albedo = tex2D(_MainTex, i.uv);


			fixed3 diffuse = _LightColor0.rgb*albedo*max(0, dot(tangentLightDir, tangentNormal));

			fixed t = 1 - smoothstep(0, _DissolveWidth,dissolve - _DissolveAmount);
			fixed3 dissolveCol = lerp(_DissolveStartColor, _DissolveEndColor, t);

			UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);

			return fixed4(diffuse * atten,1);
		}

			ENDCG
		}


		Pass
		{
			Name "SHADOWCASTER"
			Cull Off
			Tags{"LightMode" = "ShadowCaster"}
			CGPROGRAM
			#pragma vertex vertshadow
			#pragma fragment fragshadow

		v2fs vertshadow(appdata_base v)
		{
			v2fs o;
			TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uvDissolve = TRANSFORM_TEX(v.texcoord, _DissolveMap);
			return o;
		}

		fixed4 fragshadow(v2fs i) :SV_TARGET
		{
			fixed dissolve = tex2D(_DissolveMap,i.uvDissolve);
			clip(dissolve - _DissolveAmount);
			SHADOW_CASTER_FRAGMENT(i);
		}
			ENDCG
		}

	}

}
