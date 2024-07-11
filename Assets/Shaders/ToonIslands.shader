Shader "Custom/ToonIslands"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_TopColor("Top Color", Color) = (1,1,1,1)
		_PeakColor("Peak Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,0,1)
		_SandColor("Sand Color", Color) = (1,1,0,1)
		_RockColor("RockColor", Color) = (0.5, 0.5, 0.5, 1)
		_MaxHeight("Max Height", Float) = 10
		_MinHeight("Min Height", Float) = 0
		_PeakHeight("Peak Height", Float) = 10
		_BeachHeight("Beach Height", Float) = 1
		_HeightSmoother("Height Smoother", Range(0,2)) = 1
		_Flatness("Flatness", Range(0,1)) = 1
		_Steepness("Steepness", Range(0,1)) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BeachStrength("Beach Strength", Float) = 10
		_DeformationMask("Deformation Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _DeformationMask;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_DeformationMask;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _TopColor;
		fixed4 _BottomColor;
		fixed4 _SandColor;
		fixed4 _RockColor;
		fixed4 _PeakColor;
		float _MaxHeight;
		float _MinHeight;
		float _HeightSmoother;
		float _Flatness;
		float _Steepness;
		float _BeachHeight;
		float _BeachStrength;
		float _PeakHeight;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.uv_DeformationMask = v.texcoord;
		}

		float4 alphaBlend(float4 top, float4 bottom)
		{
			float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
			float alpha = top.a + bottom.a * (1 - top.a);

			return float4(color, alpha);
		}

		float3 calculateContrib(float height, float normContrib, float maskValue)
		{
			if (height <= _MinHeight) {
				return _SandColor.rgb;
			}

			float heightDiff = _MaxHeight - _MinHeight;
			float proportion = pow(height/heightDiff, _HeightSmoother);
			float4 top = _TopColor * proportion;
			float4 bottom = _BottomColor * (1 - proportion);
			float4 finalColor;
			if (height < _BeachHeight && normContrib > _Flatness) {
				finalColor = _SandColor;
			} else {
				if (normContrib < _Steepness)
				{
					finalColor = _RockColor;
				}
				else {
					if (height < _PeakHeight)
					{
						finalColor = _TopColor;
					}
					else {
						finalColor = _PeakColor;
					}
					
				}
			}

			if (maskValue > 0) {
				if (height > _BeachHeight) {
					if (normContrib < _Steepness)
					{
						finalColor = _RockColor;
					} else if (height > _PeakHeight) {
						finalColor = _PeakColor;
					} else {
						finalColor = _BottomColor;
					} 
				}
				else {
					finalColor = _SandColor;
				}
				
			}

			return finalColor.rgb;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float3 normal = normalize(IN.worldNormal);
			float NdotU = dot(normal, float3(0, 1, 0));
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float maskValue = tex2D(_DeformationMask, IN.uv_DeformationMask).r;
            o.Albedo = calculateContrib(IN.worldPos.y, NdotU, maskValue);	
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
