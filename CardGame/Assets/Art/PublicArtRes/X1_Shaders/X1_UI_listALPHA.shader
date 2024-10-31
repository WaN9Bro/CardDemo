// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
//Blend SrcAlpha OneMinusSrcAlpha
Shader "X1/UI/list_alpha"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Stnecil("Stnecil", Float) = 0
		_StencilComp("StencilComp", Float) = 0
		_TitleX("TitleX", Range(1 , 10)) = 1
		_TitleY("TitleY", Range(1 , 10)) = 1
		_Speed("Speed", Range(0.001 , 1)) = 1

	}

		SubShader
		{


			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaToMask Off
			Cull Off
			ColorMask RGBA
			ZWrite Off
			ZTest Always
			Offset 0 , 0
			Stencil
			{
				Ref[_Stnecil]
				Comp[_StencilComp]
				Pass Keep
				Fail Keep
				ZFail Keep
			}


			Pass
			{
				Name "Unlit"
				Tags { "LightMode" = "ForwardBase" }
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

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP

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

				float4 worldPos : TEXCOORD0;

				float4 ase_texcoord1 : TEXCOORD1;
				//--------------
				//float4 worldPosition : TEXCOORD1;
				half4  mask : TEXCOORD2;
				//--------------
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _StencilComp;
			uniform float _Stnecil;
			uniform sampler2D _MainTex;
			uniform float _TitleX;
			uniform float _TitleY;
			uniform float _Speed;

			//增加区域属性++++++++++
			float4 _ClipRect;
			//float4 _MainTex_ST;
			float _MaskSoftnessX;
			float _MaskSoftnessY;


			v2f vert(appdata v)
			{
				//因为没有做任何顶点的操作，所以直接整段复制过来用，o.worldPos 用的是float4  需要在V2F中更改 另外V2F中也有更改
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				float4 vPosition = UnityObjectToClipPos(v.vertex);
				o.worldPos = v.vertex;
				o.vertex = vPosition;



				float2 pixelSize = vPosition.w;
				pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
				float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
				float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);





				o.ase_texcoord1.xy = v.ase_texcoord.xy;

				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = float2(maskUV.x, maskUV.y);
				o.mask = half4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + abs(pixelSize.xy)));
				float3 vertexValue = float3(0, 0, 0);



				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float temp_output_105_0 = (_TitleX * _TitleY * frac((_Time.y * _Speed)));
				float X109 = _TitleX;
				float2 texCoord102 = i.ase_texcoord1.xy * float2(1,1) + float2(0,0);
				float width118 = (texCoord102.x / X109);
				float Y110 = _TitleY;
				float height119 = (texCoord102.y / Y110);
				float2 appendResult130 = (float2(((floor(fmod(temp_output_105_0 , X109)) * (1.0 / X109)) + width118) , ((floor((temp_output_105_0 / X109)) * (1.0 / Y110)) + height119)));
				float4 MainRGBA75 = tex2D(_MainTex, appendResult130);


				finalColor = MainRGBA75;

				//最后的合成+++++++++
				#ifdef UNITY_UI_CLIP_RECT
				half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);
				finalColor.a *= m.x * m.y;
				#endif

				#ifdef UNITY_UI_ALPHACLIP
				clip(finalColor.a - 0.001);
				#endif

				finalColor.a *= finalColor.a;
				return finalColor;

			}
			ENDCG
		}
		}
			CustomEditor "ASEMaterialInspector"


}
/*ASEBEGIN
Version=18707
-1094;6;1021;1013;-625.8387;3537.777;1.84582;True;False
Node;AmplifyShaderEditor.RangedFloatNode;108;-1158.997,-2461.955;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;False;0;False;1;1;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;106;-1119.75,-2618.921;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-843.2753,-2555.529;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-981.7112,-2895.226;Inherit;False;Property;_TitleX;TitleX;3;0;Create;True;0;0;False;0;False;1;4;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-977.95,-2791.741;Inherit;False;Property;_TitleY;TitleY;4;0;Create;True;0;0;False;0;False;1;6;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;107;-686.9897,-2635.161;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;-530.0259,-3080.439;Inherit;False;X;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;102;-573.8062,-3331.325;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;125;-156.7184,-2385.156;Inherit;False;109;X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-530.0251,-2956.756;Inherit;False;Y;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-383.9897,-2648.547;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-85.39716,-2666.975;Inherit;False;109;X;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FmodOpNode;124;85.32228,-2787.053;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;116;-247.1755,-3238.602;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;-28.83073,-2314.864;Inherit;False;110;Y;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;112;33.9165,-2468.049;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;117;-247.1755,-3111.876;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;114;235.3954,-2808.749;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;133;209.0392,-2361.195;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-96.1198,-3125.055;Inherit;False;height;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;132;268.5975,-2692.585;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;126;213.5819,-2469.931;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;118;-101.1889,-3235.562;Inherit;False;width;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;501.1017,-2652.494;Inherit;False;118;width;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;553.7297,-2809.764;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;437.1821,-2471.23;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;490.6014,-2322.199;Inherit;False;119;height;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;129;714.0818,-2502.431;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;123;711.8833,-2764.141;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;130;945.8497,-2649.894;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;68;1140.868,-2684.734;Inherit;True;Property;_MainTex;MainTex;0;0;Create;False;0;0;True;0;False;-1;None;a21f6fd6bb695ba40997f6d61460ddc3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;75;1565.862,-2657.203;Inherit;False;MainRGBA;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-207.6943,-3357.984;Inherit;False;Property;_StencilComp;StencilComp;2;0;Create;True;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-194.72,-3456.306;Inherit;False;Property;_Stnecil;Stnecil;1;0;Create;True;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1909.62,-2619.085;Float;False;True;-1;2;ASEMaterialInspector;100;1;X1/UI/list_alpha;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;True;255;True;64;255;False;-1;255;False;-1;7;True;65;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;7;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;134;0;106;0
WireConnection;134;1;108;0
WireConnection;107;0;134;0
WireConnection;109;0;103;0
WireConnection;110;0;104;0
WireConnection;105;0;103;0
WireConnection;105;1;104;0
WireConnection;105;2;107;0
WireConnection;124;0;105;0
WireConnection;124;1;113;0
WireConnection;116;0;102;1
WireConnection;116;1;109;0
WireConnection;112;0;105;0
WireConnection;112;1;125;0
WireConnection;117;0;102;2
WireConnection;117;1;110;0
WireConnection;114;0;124;0
WireConnection;133;1;135;0
WireConnection;119;0;117;0
WireConnection;132;1;113;0
WireConnection;126;0;112;0
WireConnection;118;0;116;0
WireConnection;121;0;114;0
WireConnection;121;1;132;0
WireConnection;127;0;126;0
WireConnection;127;1;133;0
WireConnection;129;0;127;0
WireConnection;129;1;128;0
WireConnection;123;0;121;0
WireConnection;123;1;122;0
WireConnection;130;0;123;0
WireConnection;130;1;129;0
WireConnection;68;1;130;0
WireConnection;75;0;68;0
WireConnection;1;0;75;0
ASEEND*/
//CHKSM=311540925A43276FBA5F40B3F05AA61CC0361E2C