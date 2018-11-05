Shader "Particles/Additive" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	
		//-------------------add----------------------
		  _MinX ("Min X", Float) = -10
		  _MaxX ("Max X", Float) = 10
		  _MinY ("Min Y", Float) = -10
		  _MaxY ("Max Y", Float) = 10
		  //-------------------add----------------------
	
	}
 
	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGBA
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
		SubShader {
			Pass {
		
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
 
				#include "UnityCG.cginc"
 
				sampler2D _MainTex;
				fixed4 _TintColor;
				//-------------------add----------------------
				float _MinX;
				float _MaxX;
				float _MinY;
				float _MaxY;
				//-------------------add----------------------
			
				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
 
				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
					#endif
					//-------------------add----------------------
					float3 vpos : TEXCOORD2;
					//-------------------add----------------------
				};
			
				float4 _MainTex_ST;
 
				v2f vert (appdata_t v)
				{
					v2f o;
					//-------------------add----------------------
					o.vpos = v.vertex.xyz;
					//-------------------add----------------------
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
 
				sampler2D_float _CameraDepthTexture;
				float _InvFade;
			
				fixed4 frag (v2f i) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON
					float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
					#endif
				
					//-------------------add----------------------
					 fixed4 c =2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
					 c.a *= (i.vpos.x >= _MinX );
	           		 c.a *= (i.vpos.x <= _MaxX);
					 c.a *= (i.vpos.y >= _MinY);
					 c.a *= (i.vpos.y <= _MaxY);
					 c.rgb *= c.a;

			//		 if(c.rgb <0.2)  
			//	{  
			//		c.a = 0;  
			//	}  

					return c;
					//-------------------add----------------------
				}
				ENDCG 
			}
		}	
	}
}