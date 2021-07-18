Shader "Game/Effect/Dissolve_Diffuse_Base"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount",Range(0,1))=.5
		_DissolveMap("Dissolve Map",2D)="white"{}
		_DissolveWidth("Dissolve Width",Range(0,0.2)) = .1
		_DissolveStartColor("Dissolve Start Color",Color) = (1,0,0,1)
		_DissolveEndColor("Dissolve End Color",Color) = (0,1,0,1)
	}
	SubShader
	{
		Tags {"RenderType"="Gemotry" "RenderQueue"="Opaque"}
		
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "Lighting.cginc"

		sampler2D _MainTex;
		half4 _MainTex_ST;
		fixed _DissolveAmount;
		sampler2D _DissolveMap;
		half4 _DissolveMap_ST;
		fixed _DissolveWidth;
		fixed4 _DissolveStartColor;
		fixed4 _DissolveEndColor;

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal:NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uvDissolve:TEXCOORD2;
			float3 lightDir:TEXCOORD3;
			float3 worldPos:TEXCOORD4;
			float3 worldNormal:TEXCOORD5;
			SHADOW_COORDS(6)
		};		

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uvDissolve = TRANSFORM_TEX(v.uv, _DissolveMap);

			o.lightDir =  WorldSpaceLightDir(v.vertex).xyz;
			o.worldPos =mul(unity_ObjectToWorld,v.vertex);
			o.worldNormal = mul(v.normal, unity_WorldToObject);
			TRANSFER_SHADOW(o);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed dissolve = tex2D(_DissolveMap,i.uvDissolve).r;
			clip(dissolve - _DissolveAmount);


			fixed3 albedo = tex2D(_MainTex, i.uv);

			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*albedo;

			fixed3 diffuse = _LightColor0.rgb*albedo*max(0, dot(i.lightDir, i.worldNormal));

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

			ENDCG
		}
		UsePass "Game/Effect/Dissolve_Diffuse_Full/SHADOWCASTER"
	}

}
