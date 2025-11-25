Shader "Unlit/451Shader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
			#pragma vertex MyVert
			#pragma fragment MyFrag
			
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
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 vertexWC : TEXCOORD3;
			};

            // our own matrix
            float4x4 MyXformMat;  // our own transform matrix!!
            fixed4   MyColor;

			// Texture support
			sampler2D _MainTex;
			float4 _MainTex_ST;

			
			// diffuse lights
            float4 DiffusePosition;
			int UseDiffuseLight;

			// point light
			float4 PointLightPosition[30]; // max of 30 point lights
            fixed4 LightColor;
            float  LightNear;
            float  LightFar;
			int UsePointLight;
			
			v2f MyVert (appdata v)
			{
				v2f o;

                o.vertex = mul(MyXformMat, v.vertex);  // use our own transform matrix!
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);   // camera transform only                
				
				// Texture support
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.vertexWC = mul(UNITY_MATRIX_M, v.vertex); // this is in WC space!

				// this is not pretty but we don't have access to inverse-transpose ...
                float3 p = v.vertex + v.normal;
                p = mul(UNITY_MATRIX_M, float4(p, 1));  // now in WC space
                o.normal = normalize(p - o.vertexWC); // NOTE: this is in the world space!!
				return o;
			}
			
			// our own function
            fixed4 ComputeDiffuse(v2f i) 
			{
				if (UseDiffuseLight)
				{
					float3 l = normalize(DiffusePosition - i.vertexWC);
					return clamp(dot(i.normal, l), 0, 1);
				}
                return 0;
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
							float strength = 1;
                
							float ndotl = clamp(dot(i.normal, l), 0, 1);
							if (d > LightNear) 
							{
								if (d < LightFar) 
								{
									float range = LightFar - LightNear;
									float n = d - LightNear;
									strength = smoothstep(0, 1, 1.0 - (n*n) / (range*range));
								}
								else 
								{
									strength = 0;
								}
							}
							lightValue += ndotl * strength;
						}
						
					}
					
				}
                return lightValue;
            }

			fixed4 MyFrag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * MyColor;
                fixed4 difLight = ComputeDiffuse(i);
				fixed4 pointLight = (ComputePointLight(i) * LightColor);
				return col + (difLight + pointLight);
			}
			ENDCG
		}
	}
}