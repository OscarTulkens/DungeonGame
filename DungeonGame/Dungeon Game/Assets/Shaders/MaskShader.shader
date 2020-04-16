Shader "Custom/StencilMask"
{

	Properties{}

		SubShader{

		Tags {
		 "RenderType" = "Opaque"
		 }

		 Pass{
		 ZWrite Off
		 }
	}
}