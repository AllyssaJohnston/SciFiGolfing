Shader "Custom/StandardShaderWithLighting"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
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

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

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
			#pragma vertex vert
			#pragma fragment frag
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
                float3 vertexWC : TEXCOORD3;
			};

			sampler2D _MainTex;
            fixed4 _Color;

			// diffuse lights
            float4 LightPosition;
			int UseDiffuseLight;
			

			// point light
			float4 PointLightPosition;
            fixed4 LightColor;
            float  LightNear;
            float  LightFar;
			int UsePointLight;

            float minDifffuse;
            float noDiffuse;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);   // World to NDC

				o.uv = v.uv; // no specific placement support

                o.vertexWC = mul(UNITY_MATRIX_M, v.vertex); // this is in WC space!
                // this is not pretty but we don't have access to inverse-transpose ...
                float3 p = v.vertex + v.normal;
                p = mul(UNITY_MATRIX_M, float4(p, 1));  // now in WC space
                o.normal = normalize(p - o.vertexWC); // NOTE: this is in the world space!!
				return o;
			}
			
            // our own function
            fixed4 ComputeDiffuse(v2f i) {
				if (UseDiffuseLight)
				{
					float3 l = normalize(LightPosition - i.vertexWC);
					return clamp(dot(i.normal, l), minDifffuse, 1);
				}
  
                return noDiffuse;
            }

			// our own function
            fixed4 ComputePointLight(v2f i) {           
				if (UsePointLight)
				{
					float3 l5 = normalize(PointLightPosition - i.vertexWC);
					float d = length(l5);
					l5 = l5 / d;
					float strength = 1;
                
					float ndotl = clamp(dot(i.normal, l5), 0, 1);
					if (d > LightNear) {
						if (d < LightFar) {
							float range = LightFar - LightNear;
							float n = d - LightNear;
							strength = smoothstep(0, 1, 1.0 - (n*n) / (range*range));
						}
						else {
							strength = 0;
						}
					}
					return ndotl * strength;
				}
                return 0;
            }

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                fixed4 colWithLight = (col * ComputeDiffuse(i)) + (col * ComputePointLight(i) * LightColor); // add in lighting
				return colWithLight;
			}

			ENDCG

        }
    }
    FallBack "Diffuse"
}
