Shader "Custom/RB_GlobeCutoutShader_mobile" {

	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_HotlineColor("Hotline Color", Color) = (1,1,1,1)
		_MainTex("Albedo 1 (RGB)", 2D) = "white" {}
		_MainTex2("Albedo 2 (RGB)", 2D) = "white" {}
		_MainTex3("Albedo 3 (RGB)", 2D) = "white" {}
		_HotlineTex("Hotline (RGB)", 2D) = "white" {}
		_MaskTex("Mask", 2D) = "white" {}
		_CutoutTex("Cutout (RGB)", 2D) = "white" {}
		_Progress("Progress", range(0, 1)) = .5
	}

	SubShader{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
		Blend SrcAlpha OneMinusSrcAlpha 
		Lighting Off //For the settings defined in the Material block to have any effect, you must enable Lighting with the Lighting On command. If lighting is off instead, the color is taken straight from the Color command.
		ZWrite Off //avoid writing to z buffer because we don not pack any depth here and mesh is flat
		//ZWrite Off Controls whether pixels from this object are written to the depth buffer (default is On). If you’re drawing solid objects, leave this on. If you’re drawing semitransparent effects, switch to ZWrite Off. For more details read below.

		CGPROGRAM
		#pragma target 3.5
#pragma surface surf NoLighting alpha:blend
		//alpha:blend is key because shader uses mask texture's alpha for "filling" mechanism

		//Custom lightning function to prevent any light from affecting the globe
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

	sampler2D _MaskTex;
	sampler2D _MainTex;
	sampler2D _MainTex2;
	sampler2D _MainTex3;
	sampler2D _HotlineTex;
	sampler2D _CutoutTex;

	float _Progress;

	fixed4 _Color;
	fixed4 _HotlineColor;

	struct Input {
		float2 uv_MaskTex;
		float2 uv_MainTex;
		float2 uv_MainTex2;
		float2 uv_MainTex3;
		float2 uv_HotlineTex;
		float2 uv_CutoutTex;
	};

	float2 uvx;
	float2 uvmasx;
	fixed4 c;
	fixed4 a;
	fixed4 x;
	fixed4 t;
	void surf(Input IN, inout SurfaceOutput o) {

		//place the hotline texture to the where the progress value is 
		uvx = float2(IN.uv_HotlineTex.x, IN.uv_HotlineTex.y - _Progress);
		//offset mask texture using the same logic from above
		uvmasx = float2(IN.uv_MaskTex.x, IN.uv_MaskTex.y - _Progress);

		//multiply textures by each other and color
		c = tex2D(_MainTex, IN.uv_MainTex) * tex2D(_MainTex2, IN.uv_MainTex2) * tex2D(_MainTex3, IN.uv_MainTex3) * _Color;
		a = tex2D(_MaskTex, uvmasx);
		x = tex2D(_HotlineTex, uvx) * _HotlineColor;
		t = tex2D(_CutoutTex, IN.uv_CutoutTex);

		//multiply color "C" by color "A" darkens down the result so we multiply it by "3" to brighten it up
		c = c  * a * 3;
		// (c * x * 5) add more color to the white hotline
		// the "2" in the "x.a * 2" once against brightens the result
		// _HotlineColor.a alters hotlines transparency
		c = c + ((c * x * 2) + x) * x.a * 2 * _HotlineColor.a;
		o.Albedo = c.rgb;
		o.Alpha = c.a * t.a;
	}
	ENDCG
	}
		Fallback "Legacy Shaders/Transparent/VertexLit" //Fallback value for the device “if none of subshaders can run on this hardware, try using the ones from another shader”.
}
