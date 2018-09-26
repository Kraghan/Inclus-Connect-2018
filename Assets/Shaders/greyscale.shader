﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "2D/Greyscale"
{
    Properties
    {
        [PerRendererData]   _MainTex("Sprite Texture", 2D) = "white" {}
        [MaterialToggle]    PixelSnap("Pixel snap", Float) = 1
        [MaterialToggle]    _isGhost("Is Ghost", Float) = 0

        _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255

         _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue"             = "Transparent"
            "IgnoreProjector"   = "True"
            "RenderType"        = "Transparent"
            "PreviewType"       = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON

            sampler2D _MainTex;

            float _isGhost;

            struct Vertex
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct Fragment
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            Fragment vert(Vertex v)
            {
                Fragment o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = v.uv_MainTex;
                o.uv2 = v.uv2;

                return o;
            }

            float4 frag(Fragment IN) : COLOR
            {
                float4 o = float4(1, 0, 0, 0.2);

                half4 c = tex2D(_MainTex, IN.uv_MainTex);

                if(_isGhost == 0)
                {
                    return c;
                }

                float grey = (0.2126f * c.r + 0.7152f * c.g + 0.0722f * c.b);
				o.rgb = float3(grey, grey, grey);

                return o;
            }

            ENDCG
        }
    }
}
