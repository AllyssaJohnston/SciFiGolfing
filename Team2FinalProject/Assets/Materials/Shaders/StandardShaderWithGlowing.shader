// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/StandardShaderWithGlowing"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		// Glowing golf properties
		_GlowTex ("Glow Texture", 2D) = "white"{}
		_GlowIntensity ("Glow Intensity", Range(0,5)) = 1.0
		_ScrollSpeed("Glow Scroll Speed", Range(-5,5)) = 0.5
		_PulseSpeed ("Glow Pulse Speed", Range(0,10)) = 2.0
		_GlowEnabled ("Glow enabled", Float) = 1.0

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _GlowTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		float _GlowIntensity;
		float _ScrollSpeed;
		float _PulseSpeed;
		float _GlowEnabled;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG

        Pass 
        {
            CGPROGRAM
			#pragma vertex vert alpha
			#pragma fragment frag alpha
			// make fog work
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
                float4 vertexWC : TEXCOORD3;
			};

			sampler2D _MainTex;
			sampler2D _GlowTex;
            fixed4 _Color;

			// diffuse lights
            float4 DiffusePosition;
			int UseDiffuseLight;
			

			// point light

			float4 PointLightPosition[30]; // max of 30 point lights
            float4 PointNearFar[30]; 
			float4 PointLightColor[30];
			int UsePointLight;

            float minDiffuse;
            float noDiffuse;

			float maxPoint;
			
			//glow params
			float _GlowIntensity;
			float _ScrollSpeed;
			float _PulseSpeed;
			float _GlowEnabled;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);   // World to NDC

				o.uv = v.uv; // no specific placement support

                o.vertexWC = mul(unity_ObjectToWorld, v.vertex); // WC space
                // this is not pretty but we don't have access to inverse-transpose ...
				float3 p = v.vertex + v.normal;
                //float3 p = v.vertex + v.normal;
                p = mul(unity_ObjectToWorld, float4(p, 1));  // WC space
                o.normal = normalize(p - o.vertexWC);
				return o;
			}
			
            // our own function
            fixed4 ComputeDiffuse(v2f i) {
				if (UseDiffuseLight)
				{
					float3 l = normalize(DiffusePosition - i.vertexWC);
					return clamp(dot(i.normal, l), minDiffuse, 1);
				}
  
                return noDiffuse;
            }

			// our own function
            fixed4 ComputePointLight(v2f i) 
			{      
				float lightValue = 0;
				if (UsePointLight)
				{
					for (int count = 0; count < 30; count++)
					{
                        if (PointLightPosition[count].x <= 400.0)
						{
							float3 l = PointLightPosition[count].xyz - i.vertexWC;
							float d = length(l);
							l = l / d;
							float strength = maxPoint;
                
							float ndotl = clamp(dot(i.normal, l), 0, 1);
							float LightNear = PointNearFar[count].x;
							float LightFar = PointNearFar[count].y;
							float4 LightColor = PointLightColor[count];
							if (d > LightNear) 
							{
								if (d < LightFar) 
								{
									float range = LightFar - LightNear;
									float n = d - LightNear;
									strength = smoothstep(0, maxPoint, maxPoint - (n*n) / (range*range));
								}
								else 
								{
									strength = 0;
								}
							}
							lightValue += ndotl * strength * LightColor;
						}
						
					}
					
				}
                return lightValue;
            }

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color + fixed4(.7, .7, .7, 0);
				fixed4 difLight = ComputeDiffuse(i);
				fixed4 pointLight = ComputePointLight(i);

				// Switch of Glowing return to normal lighting
				if (_GlowEnabled < 0.5)
				{
					fixed4 blended = col * (difLight + pointLight);
					return fixed4(blended.r, blended.g, blended.b, 1);
				}
				else
				{
					// Switch on Glowing, UV scrolling (translate in MP6)
					float2 glowUV = i.uv;
					glowUV.x += _ScrollSpeed * _Time.y;
					float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5; // pulsing emission
					fixed4 glowCol = (tex2D(_GlowTex, glowUV) * _Color * _GlowIntensity * pulse) + fixed4(.3, .3, .3, 0);

					
					

					fixed4 combinedCol = col * (difLight + pointLight); 
					float whiteAlpha = 1.0 - (glowCol.a + combinedCol.a);
					if (whiteAlpha < 0) { whiteAlpha = 0; }
					fixed4 blended =  fixed4(glowCol.r * glowCol.a,			glowCol.g * glowCol.a,			glowCol.b * glowCol.a,			0) 
									+ fixed4(combinedCol.r * combinedCol.a, combinedCol.g * combinedCol.a,	combinedCol.b * combinedCol.a,	0) 
									+ fixed4(0,								1 * whiteAlpha,					1 * whiteAlpha,					1);

					return blended;
				}

				
                
			}

			ENDCG

        }
    }
    FallBack "Diffuse"
}
