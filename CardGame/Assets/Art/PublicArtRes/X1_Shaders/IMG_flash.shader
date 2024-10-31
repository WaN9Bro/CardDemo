// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MS/IMG/flash"
{
	Properties
	{
		_HightTex("HightTex", 2D) = "white" {}
		_MainTex("MainTex", 2D) = "white" {}
		_HL_MASK("HL_MASK", 2D) = "black" {}
		_width("width", Range( 1 , 60)) = 6
		_height("height", Range( 1 , 60)) = 6
		[HDR]_HLColor("HLColor", Color) = (1,1,1,1)
		[HDR]_Color("Color", Color) = (1,1,1,0)
		_downLight("downLight", Range( 0 , 1)) = 0.1
		_TimeScale("TimeScale", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _TimeScale;
			uniform sampler2D _HL_MASK;
			uniform float4 _HL_MASK_ST;
			uniform float _downLight;
			uniform float4 _HLColor;
			uniform sampler2D _HightTex;
			uniform float _width;
			uniform float _height;
			uniform float4 _Color;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 MainTex64 = tex2D( _MainTex, uv_MainTex );
				float mulTime44 = _Time.y * _TimeScale;
				float2 uv_HL_MASK = i.ase_texcoord1.xy * _HL_MASK_ST.xy + _HL_MASK_ST.zw;
				float3 appendResult58 = (float3(_HLColor.r , _HLColor.g , _HLColor.b));
				float3 lerpResult59 = lerp( (MainTex64).rgb , ( (_downLight + (abs( ( frac( ( mulTime44 + tex2D( _HL_MASK, uv_HL_MASK ).g ) ) + -0.5 ) ) - 0.0) * (1.0 - _downLight) / (0.5 - 0.0)) * appendResult58 ) , tex2D( _HL_MASK, uv_HL_MASK ).r);
				float4 appendResult62 = (float4(lerpResult59 , (MainTex64).a));
				float4 spineColor52 = appendResult62;
				float2 texCoord80 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult37 = (float2(_width , _height));
				float2 temp_output_6_0 = ( floor( ( texCoord80 * appendResult37 ) ) / appendResult37 );
				float temp_output_11_0 = distance( temp_output_6_0 , float2( 0.5,0.5 ) );
				float temp_output_20_0 = (-0.1 + (( i.ase_color.a * _Color.a ) - 0.0001) * (1.0 - -0.1) / (1.0 - 0.0001));
				float temp_output_15_0 = step( temp_output_11_0 , ( temp_output_20_0 + 0.1 ) );
				float3 appendResult34 = (float3(_Color.r , _Color.g , _Color.b));
				float3 appendResult40 = (float3(i.ase_color.r , i.ase_color.g , i.ase_color.b));
				float3 clampResult43 = clamp( ( (spineColor52).xyz + ( tex2D( _HightTex, temp_output_6_0 ).r * ( ( 1.0 - step( temp_output_11_0 , temp_output_20_0 ) ) * temp_output_15_0 ) * appendResult34 * appendResult40 ) ) , float3( 0,0,0 ) , float3( 4,4,4 ) );
				float4 appendResult31 = (float4(clampResult43 , ( (spineColor52).w * temp_output_15_0 )));
				float4 appendResult76 = (float4(appendResult31));
				
				
				finalColor = appendResult76;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18707
-1061;511;1061;841;1934.37;584.0753;1.421;True;True
Node;AmplifyShaderEditor.RangedFloatNode;73;-2000.701,-1759.765;Inherit;False;Property;_TimeScale;TimeScale;8;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-1781.796,-1591.645;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;False;-1;None;79fbe9b73b3773849898fbbfde1d9e70;True;0;False;black;Auto;False;Instance;50;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;44;-1698.168,-1753.625;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-1354.751,-1703.917;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;45;-1124.604,-1741.607;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-2235.097,-160.7639;Inherit;False;Property;_width;width;3;0;Create;True;0;0;False;0;False;6;40;1;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2234.754,-78.52639;Inherit;False;Property;_height;height;4;0;Create;True;0;0;False;0;False;6;20;1;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;80;-2255.253,-729.5602;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-945.8542,-1728.5;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-1412.4,-2353.074;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;None;79fbe9b73b3773849898fbbfde1d9e70;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;37;-1862.328,-178.7172;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1681.63,-349.777;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;-1003.673,-2335.705;Inherit;False;MainTex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;47;-700.4405,-1722.354;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;-533.8238,-1443.98;Inherit;False;Property;_HLColor;HLColor;5;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;9.734288,4.943591,1.528946,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-851.8282,-1480.811;Inherit;False;Property;_downLight;downLight;7;0;Create;True;0;0;False;0;False;0.1;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;5;-1502.49,-348.4921;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1544.294,346.7278;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;False;0.1;0.4235294;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;38;-1770.767,-35.75591;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;67;25.97335,-1549.829;Inherit;False;64;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-236.8765,-1433.348;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;48;-445.8726,-1741.606;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;0.1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-1803.959,378.3792;Inherit;False;Property;_Color;Color;6;1;[HDR];Create;True;0;0;False;0;False;1,1,1,0;0,2.617026,16.37664,0.6117647;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1498.396,159.9418;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-1321.969,-352.4017;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;22;-1387.857,238.185;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;37.19889,-1821.797;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;68;234.7294,-1543.705;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;50;17.8377,-1430.687;Inherit;True;Property;_HL_MASK;HL_MASK;2;0;Create;True;0;0;False;0;False;-1;None;79fbe9b73b3773849898fbbfde1d9e70;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;69;269.1134,-1166.485;Inherit;False;64;MainTex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;70;563.4774,-1300.71;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;11;-1053.251,-158.9578;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;642.627,-1569.301;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-1215.857,113.185;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.0001;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;12;-752.9351,-91.7986;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;62;828.8297,-1437.17;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-963.8572,254.185;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.32;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;1029.331,-1434.281;Inherit;False;spineColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;15;-794.9397,206.0469;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;18;-524.5673,-149.2874;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-279.0615,-60.18651;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-165.4782,410.6253;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;1;-931.5583,-430.6955;Inherit;True;Property;_HightTex;HightTex;0;0;Create;True;0;0;False;0;False;-1;3e13f492eb5e87643a5ba093853c6963;3e13f492eb5e87643a5ba093853c6963;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;40;-1361.416,-86.62619;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-513.022,-854.069;Inherit;False;52;spineColor;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;53;-194.3525,-982.3376;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;145.1858,-62.13976;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;354.3029,-903.8959;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;54;-185.3957,-693.2143;Inherit;False;False;False;False;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;153.5657,-522.3114;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;556.4399,-897.3596;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;4,4,4;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;816.1056,-710.379;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PosVertexDataNode;79;867.5152,-548.6113;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;76;1107.03,-557.5952;Inherit;False;FLOAT4;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;27;-1994.713,-437.1098;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;77;-2231.417,-516.3315;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1235.298,-700.9788;Float;False;True;-1;2;ASEMaterialInspector;100;1;MS/IMG/flash;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;44;0;73;0
WireConnection;55;0;44;0
WireConnection;55;1;74;2
WireConnection;45;0;55;0
WireConnection;46;0;45;0
WireConnection;37;0;4;0
WireConnection;37;1;36;0
WireConnection;3;0;80;0
WireConnection;3;1;37;0
WireConnection;64;0;23;0
WireConnection;47;0;46;0
WireConnection;5;0;3;0
WireConnection;58;0;56;1
WireConnection;58;1;56;2
WireConnection;58;2;56;3
WireConnection;48;0;47;0
WireConnection;48;3;49;0
WireConnection;41;0;38;4
WireConnection;41;1;33;4
WireConnection;6;0;5;0
WireConnection;6;1;37;0
WireConnection;22;0;17;0
WireConnection;57;0;48;0
WireConnection;57;1;58;0
WireConnection;68;0;67;0
WireConnection;70;0;69;0
WireConnection;11;0;6;0
WireConnection;59;0;68;0
WireConnection;59;1;57;0
WireConnection;59;2;50;1
WireConnection;20;0;41;0
WireConnection;20;3;22;0
WireConnection;12;0;11;0
WireConnection;12;1;20;0
WireConnection;62;0;59;0
WireConnection;62;3;70;0
WireConnection;16;0;20;0
WireConnection;16;1;17;0
WireConnection;52;0;62;0
WireConnection;15;0;11;0
WireConnection;15;1;16;0
WireConnection;18;0;12;0
WireConnection;19;0;18;0
WireConnection;19;1;15;0
WireConnection;34;0;33;1
WireConnection;34;1;33;2
WireConnection;34;2;33;3
WireConnection;1;1;6;0
WireConnection;40;0;38;1
WireConnection;40;1;38;2
WireConnection;40;2;38;3
WireConnection;53;0;51;0
WireConnection;28;0;1;1
WireConnection;28;1;19;0
WireConnection;28;2;34;0
WireConnection;28;3;40;0
WireConnection;32;0;53;0
WireConnection;32;1;28;0
WireConnection;54;0;51;0
WireConnection;24;0;54;0
WireConnection;24;1;15;0
WireConnection;43;0;32;0
WireConnection;31;0;43;0
WireConnection;31;3;24;0
WireConnection;76;0;31;0
WireConnection;27;0;77;1
WireConnection;27;1;77;2
WireConnection;0;0;76;0
ASEEND*/
//CHKSM=117EDD32980BA9FEE0E32AC2F2CB83735424FDCA