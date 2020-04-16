// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/HoleShader"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_AlphaSize("AlphaSize", Float) = 1
		_ParallaxTexture("ParallaxTexture", 2D) = "white" {}
		_ParallaxShape("ParallaxShape", 2D) = "white" {}
		_CenterColor("CenterColor", Color) = (0,0,0,0)
		_InnerEdgeColor("InnerEdgeColor", Color) = (0,0,0,0)
		_OuterEdgeColor("OuterEdgeColor", Color) = (0,0,0,0)
		_SmoothstepMin("SmoothstepMin", Float) = 0
		_SmoothstepMax("SmoothstepMax", Float) = 0
		_GradientCenter("GradientCenter", Vector) = (0,1,0,0)
		_GradientBrightness("GradientBrightness", Float) = 0.13
		_GradientMagnitude("GradientMagnitude", Float) = 1.18
		_GroundColor("GroundColor", Color) = (0.6132076,0.2239404,0,0)
		_HolePosition("HolePosition", Vector) = (0,-0.32,0,0)
		_HoleDepth("HoleDepth", Float) = 0.86

	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		
		Cull Back
		HLSLINCLUDE
		#pragma target 3.5
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero , One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 PlayerPosition;
			float CurveMultiplier;
			sampler2D _ParallaxShape;
			sampler2D _ParallaxTexture;
			CBUFFER_START( UnityPerMaterial )
			float4 _CenterColor;
			float4 _InnerEdgeColor;
			float4 _OuterEdgeColor;
			float2 _GradientCenter;
			float _GradientBrightness;
			float _GradientMagnitude;
			float _SmoothstepMin;
			float _SmoothstepMax;
			float4 _ParallaxTexture_ST;
			float4 _GroundColor;
			float2 _HolePosition;
			float _HoleDepth;
			float _AlphaSize;
			CBUFFER_END


			inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
			{
				float3 result = 0;
				int stepIndex = 0;
				int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, saturate( dot( normalWorld, viewWorld ) ) );
				float layerHeight = 1.0 / numSteps;
				float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
				uvs += refPlane * plane;
				float2 deltaTex = -plane * layerHeight;
				float2 prevTexOffset = 0;
				float prevRayZ = 1.0f;
				float prevHeight = 0.0f;
				float2 currTexOffset = deltaTex;
				float currRayZ = 1.0f - layerHeight;
				float currHeight = 0.0f;
				float intersection = 0;
				float2 finalTexOffset = 0;
				while ( stepIndex < numSteps + 1 )
				{
			currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
			if ( currHeight > currRayZ )
			{
				stepIndex = numSteps + 1;
			}
			else
			{
				stepIndex++;
				prevTexOffset = currTexOffset;
				prevRayZ = currRayZ;
				prevHeight = currHeight;
				currTexOffset += deltaTex;
				currRayZ -= layerHeight;
			}
				}
				int sectionSteps = 2;
				int sectionIndex = 0;
				float newZ = 0;
				float newHeight = 0;
				while ( sectionIndex < sectionSteps )
				{
			intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
			finalTexOffset = prevTexOffset + intersection * deltaTex;
			newZ = prevRayZ - intersection * layerHeight;
			newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
			if ( newHeight > newZ )
			{
				currTexOffset = finalTexOffset;
				currHeight = newHeight;
				currRayZ = newZ;
				deltaTex = intersection * deltaTex;
				layerHeight = intersection * layerHeight;
			}
			else
			{
				prevTexOffset = finalTexOffset;
				prevHeight = newHeight;
				prevRayZ = newZ;
				deltaTex = ( 1 - intersection ) * deltaTex;
				layerHeight = ( 1 - intersection ) * layerHeight;
			}
			sectionIndex++;
				}
				return uvs + finalTexOffset;
			}
			

			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 transform53_g1 = mul(GetObjectToWorldMatrix(),float4( v.vertex.xyz , 0.0 ));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float4 appendResult65_g1 = (float4(( ase_worldPos.x * 0.0 ) , ase_worldPos.y , ase_worldPos.z , 0.0));
				float4 transform56_g1 = mul(GetWorldToObjectMatrix(),( transform53_g1 + float4( ( pow( distance( appendResult65_g1 , float4( PlayerPosition , 0.0 ) ) , 2.0 ) * CurveMultiplier * float3(0,1,0) ) , 0.0 ) ));
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord5.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord6.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord7.xyz = ase_worldBitangent;
				
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				o.ase_texcoord8 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.w = 0;
				o.ase_texcoord7.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = transform56_g1.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult10 = (float2(ase_screenPosNorm.x , ase_screenPosNorm.y));
				float4 lerpResult70 = lerp( _InnerEdgeColor , _OuterEdgeColor , ( ( ( distance( appendResult10 , _GradientCenter ) * 1.16 ) + _GradientBrightness ) * _GradientMagnitude ));
				float2 uv0112 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float3 ase_worldTangent = IN.ase_texcoord5.xyz;
				float3 ase_worldNormal = IN.ase_texcoord6.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord7.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_tanViewDir =  tanToWorld0 * ase_worldViewDir.x + tanToWorld1 * ase_worldViewDir.y  + tanToWorld2 * ase_worldViewDir.z;
				ase_tanViewDir = normalize(ase_tanViewDir);
				float2 OffsetPOM37 = POM( _ParallaxTexture, uv0112, ddx(uv0112), ddy(uv0112), ase_worldNormal, ase_worldViewDir, ase_tanViewDir, 8, 8, (0.5 + (sin( _TimeParameters.x * 0.5 ) - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)), 0, _ParallaxTexture_ST.xy, float2(0,0), 0 );
				float smoothstepResult66 = smoothstep( _SmoothstepMin , _SmoothstepMax , tex2D( _ParallaxShape, OffsetPOM37, ddx( OffsetPOM37 ), ddy( OffsetPOM37 ) ).r);
				float4 lerpResult50 = lerp( _CenterColor , lerpResult70 , smoothstepResult66);
				float2 appendResult99 = (float2(IN.ase_texcoord8.xyz.x , IN.ase_texcoord8.xyz.y));
				float temp_output_106_0 = ( 1.0 - step( ( distance( appendResult99 , _HolePosition ) * _HoleDepth ) , 1.0 ) );
				float4 FakeGroundCol115 = ( _GroundColor * saturate( temp_output_106_0 ) );
				float FakeGroundAlpha116 = temp_output_106_0;
				float4 lerpResult111 = lerp( lerpResult50 , FakeGroundCol115 , FakeGroundAlpha116);
				
				float2 appendResult89 = (float2(IN.ase_texcoord8.xyz.x , IN.ase_texcoord8.xyz.y));
				float Alpha113 = ( distance( appendResult89 , float2( 0,0 ) ) * _AlphaSize );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = lerpResult111.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = Alpha113;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 PlayerPosition;
			float CurveMultiplier;
			CBUFFER_START( UnityPerMaterial )
			float4 _CenterColor;
			float4 _InnerEdgeColor;
			float4 _OuterEdgeColor;
			float2 _GradientCenter;
			float _GradientBrightness;
			float _GradientMagnitude;
			float _SmoothstepMin;
			float _SmoothstepMax;
			float4 _ParallaxTexture_ST;
			float4 _GroundColor;
			float2 _HolePosition;
			float _HoleDepth;
			float _AlphaSize;
			CBUFFER_END


			
			float3 _LightDirection;

			VertexOutput ShadowPassVertex( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 transform53_g1 = mul(GetObjectToWorldMatrix(),float4( v.vertex.xyz , 0.0 ));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float4 appendResult65_g1 = (float4(( ase_worldPos.x * 0.0 ) , ase_worldPos.y , ase_worldPos.z , 0.0));
				float4 transform56_g1 = mul(GetWorldToObjectMatrix(),( transform53_g1 + float4( ( pow( distance( appendResult65_g1 , float4( PlayerPosition , 0.0 ) ) , 2.0 ) * CurveMultiplier * float3(0,1,0) ) , 0.0 ) ));
				
				o.ase_texcoord2 = v.vertex;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = transform56_g1.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;

				return o;
			}

			half4 ShadowPassFragment(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult89 = (float2(IN.ase_texcoord2.xyz.x , IN.ase_texcoord2.xyz.y));
				float Alpha113 = ( distance( appendResult89 , float2( 0,0 ) ) * _AlphaSize );
				
				float Alpha = 1;
				float AlphaClipThreshold = Alpha113;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 PlayerPosition;
			float CurveMultiplier;
			CBUFFER_START( UnityPerMaterial )
			float4 _CenterColor;
			float4 _InnerEdgeColor;
			float4 _OuterEdgeColor;
			float2 _GradientCenter;
			float _GradientBrightness;
			float _GradientMagnitude;
			float _SmoothstepMin;
			float _SmoothstepMax;
			float4 _ParallaxTexture_ST;
			float4 _GroundColor;
			float2 _HolePosition;
			float _HoleDepth;
			float _AlphaSize;
			CBUFFER_END


			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 transform53_g1 = mul(GetObjectToWorldMatrix(),float4( v.vertex.xyz , 0.0 ));
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float4 appendResult65_g1 = (float4(( ase_worldPos.x * 0.0 ) , ase_worldPos.y , ase_worldPos.z , 0.0));
				float4 transform56_g1 = mul(GetWorldToObjectMatrix(),( transform53_g1 + float4( ( pow( distance( appendResult65_g1 , float4( PlayerPosition , 0.0 ) ) , 2.0 ) * CurveMultiplier * float3(0,1,0) ) , 0.0 ) ));
				
				o.ase_texcoord2 = v.vertex;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = transform56_g1.xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult89 = (float2(IN.ase_texcoord2.xyz.x , IN.ase_texcoord2.xyz.y));
				float Alpha113 = ( distance( appendResult89 , float2( 0,0 ) ) * _AlphaSize );
				
				float Alpha = 1;
				float AlphaClipThreshold = Alpha113;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=17900
-1920;0;1920;1019;-439.4347;898.5881;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;90;-675.728,589.671;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;89;-432.0378,671.168;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-135.2754,1012.751;Inherit;False;Property;_AlphaSize;AlphaSize;0;0;Create;True;0;0;False;0;1;2.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;83;-179.0381,683.1678;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;120.1825,677.4954;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.23;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;1491.435,-296.5881;Inherit;False;Global;CurveMultiplier;CurveMultiplier;14;0;Create;True;0;0;False;0;0;-6.976844E-14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;447.3846,671.4586;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;556.5898,165.0726;Inherit;False;Property;_SmoothstepMax;SmoothstepMax;7;0;Create;True;0;0;False;0;0;-0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;71;82.9903,-959.527;Inherit;False;Property;_OuterEdgeColor;OuterEdgeColor;5;0;Create;True;0;0;False;0;0,0,0,0;1,0.7535766,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;9;-1523.869,-775.5064;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;6;-980.8686,-700.5064;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-746.8688,-695.5064;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.16;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1233.869,-712.5064;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-348.4097,-578.427;Inherit;False;Property;_GradientBrightness;GradientBrightness;9;0;Create;True;0;0;False;0;0.13;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;82;-372.3839,-13.43358;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-81.20967,-475.827;Inherit;False;Property;_GradientMagnitude;GradientMagnitude;10;0;Create;True;0;0;False;0;1.18;4.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;70;544.4901,-730.7271;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;79;-556.4702,-12.6352;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;47;-387.8018,187.1705;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;37;40.3388,-258.0178;Inherit;False;0;8;False;-1;16;False;-1;2;0.02;0;False;1,1;False;0,0;Texture2D;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DdyOpNode;49;339.5054,-48.13083;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;19;-1518.494,-558.4992;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;108;1118.245,1286.889;Inherit;False;Property;_GroundColor;GroundColor;11;0;Create;True;0;0;False;0;0.6132076,0.2239404,0,0;0.4313725,0.3568627,0.2901961,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;20;-1200.766,-582.9229;Inherit;False;Property;_GradientCenter;GradientCenter;8;0;Create;True;0;0;False;0;0,1;0.5,0.35;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;45;-411.4233,-212.8652;Inherit;True;Property;_ParallaxTexture;ParallaxTexture;1;0;Create;True;0;0;False;0;5360b8b9f045a314a95e6c17ad87cd96;5360b8b9f045a314a95e6c17ad87cd96;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;112;-403.3572,-338.6068;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;177.4962,-705.0556;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DdxOpNode;48;340.7217,-118.6724;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;52;93.1682,-1153.84;Inherit;False;Property;_InnerEdgeColor;InnerEdgeColor;4;0;Create;True;0;0;False;0;0,0,0,0;1,0.4078431,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;115;1767.479,1620.749;Inherit;False;FakeGroundCol;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;50;1052.412,-755.1176;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;51;813.5657,-969.815;Inherit;False;Property;_CenterColor;CenterColor;3;0;Create;True;0;0;False;0;0,0,0,0;1,0.04126912,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;111;1438.928,-724.5443;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;102;1013.719,2114.39;Inherit;False;Property;_HoleDepth;HoleDepth;13;0;Create;True;0;0;False;0;0.86;1.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;551.39,74.07276;Inherit;False;Property;_SmoothstepMin;SmoothstepMin;6;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;490.1885,-279.4423;Inherit;True;Property;_ParallaxShape;ParallaxShape;2;0;Create;True;0;0;False;0;-1;5360b8b9f045a314a95e6c17ad87cd96;8c4a7fca2884fab419769ccc0355c0c1;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;99;-174.3678,2165.523;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;119;1727.839,-292.7949;Inherit;False;TerrainBend;-1;;1;1e2b703ce37c8764cbded96f80bd4b28;0;1;71;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DistanceOpNode;101;78.63163,2177.523;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;1066.72,-543.2344;Inherit;False;115;FakeGroundCol;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;103;-418.0582,2084.025;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-84.70967,-706.027;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;1052.72,-464.2344;Inherit;False;116;FakeGroundAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;1787.196,-627.1943;Inherit;False;113;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;109;1271.741,1897.852;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;1195.49,1665.278;Inherit;False;FakeGroundAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;1488.602,1625.436;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;105;684.0487,2053.74;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;347.8527,2168.851;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.23;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;100;-180.24,2279.878;Inherit;False;Property;_HolePosition;HolePosition;12;0;Create;True;0;0;False;0;0,-0.32;0,-0.29;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;1007.167,1889.724;Inherit;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;66;817.39,55.87271;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;63;1385.77,-147.0943;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;62;1385.77,-147.0943;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;59;1385.77,-147.0943;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;61;1385.77,-147.0943;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;60;2062.828,-724.5404;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Custom/HoleShader;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;7;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;3;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;11;Surface;0;  Blend;2;Two Sided;1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;Meta Pass;0;Extra Pre Pass;0;Vertex Position,InvertActionOnDeselection;0;0;5;False;True;True;True;False;False;;0
WireConnection;89;0;90;1
WireConnection;89;1;90;2
WireConnection;83;0;89;0
WireConnection;86;0;83;0
WireConnection;86;1;91;0
WireConnection;113;0;86;0
WireConnection;6;0;10;0
WireConnection;6;1;20;0
WireConnection;11;0;6;0
WireConnection;10;0;9;1
WireConnection;10;1;9;2
WireConnection;82;0;79;3
WireConnection;70;0;52;0
WireConnection;70;1;71;0
WireConnection;70;2;74;0
WireConnection;37;0;112;0
WireConnection;37;1;45;0
WireConnection;37;2;82;0
WireConnection;37;3;47;0
WireConnection;49;0;37;0
WireConnection;74;0;72;0
WireConnection;74;1;75;0
WireConnection;48;0;37;0
WireConnection;115;0;107;0
WireConnection;50;0;51;0
WireConnection;50;1;70;0
WireConnection;50;2;66;0
WireConnection;111;0;50;0
WireConnection;111;1;118;0
WireConnection;111;2;117;0
WireConnection;38;1;37;0
WireConnection;38;3;48;0
WireConnection;38;4;49;0
WireConnection;99;0;103;1
WireConnection;99;1;103;2
WireConnection;119;71;120;0
WireConnection;101;0;99;0
WireConnection;101;1;100;0
WireConnection;72;0;11;0
WireConnection;72;1;73;0
WireConnection;109;0;106;0
WireConnection;116;0;106;0
WireConnection;107;0;108;0
WireConnection;107;1;109;0
WireConnection;105;0;104;0
WireConnection;104;0;101;0
WireConnection;104;1;102;0
WireConnection;106;1;105;0
WireConnection;66;0;38;1
WireConnection;66;1;67;0
WireConnection;66;2;68;0
WireConnection;60;2;111;0
WireConnection;60;4;114;0
WireConnection;60;5;119;0
ASEEND*/
//CHKSM=0CD227C64F2C97344F1128D39B551CE775C48BF8