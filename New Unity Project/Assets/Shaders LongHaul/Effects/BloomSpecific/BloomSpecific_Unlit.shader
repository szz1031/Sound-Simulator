Shader "Game/Effect/BloomSpecific/Unlit"
{
	Properties
	{
	    _MainTex("Sprite Texture", 2D) = "white" {}
	    _Color("_Color",Color)=(1,1,1,1)
	}

	SubShader
	{ 
		Tags {"RenderType" = "BloomCommon" "IgnoreProjector" = "True" "Queue" = "Transparent" }
	Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }

		Cull Back
		USEPASS "Hidden/PostEffect/PE_BloomSpecific_Render/BLOOM_COLOR"
	}
}
