﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SpriteShadowV2" {
Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff ("Cutoff", Float) = 0.5
    }
    SubShader {
        Tags {"Queue" = "Geometry" "IgnoreProjector"="True" "RenderType" = "Transparent"}
        Cull Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
       
        Pass {
            Tags { "LightMode" = "ForwardBase" }    // handles main directional light, vertex/SH lights, and lightmaps
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_fog_exp2
            #pragma fragmentoption ARB_precision_hint_fastest
           
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
   
            float4 _MainTex_ST;
            sampler2D _MainTex;
            float _Cutoff;
 
            struct v2f {
                float4    pos            : SV_POSITION;
                float2    uv            : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
           
            v2f vert (appdata_tan v) {
                v2f o;
               
                o.pos = UnityObjectToClipPos( v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }
                   
            fixed4 frag(v2f i) : COLOR {
                fixed4 tex = tex2D(_MainTex, i.uv);
               
                // If alpha is less than _Cutoff, then bail out on this pixel entirely
                // (don't even write to the depth buffer).
                clip(tex.a - _Cutoff);
 
                fixed atten = SHADOW_ATTENUATION(i); // Attenuation for shadows ONLY.
                fixed4 c = tex * atten;        // attenuate color
                c.a = tex.a;                // don't attenuate alpha
                return c;
            }
            ENDCG
        }
        Pass {
            Tags {"LightMode" = "ForwardAdd"}    // handles per-pixel additive lights (called once per such light)
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd_fullshadows
            #pragma fragmentoption ARB_fog_exp2
            #pragma fragmentoption ARB_precision_hint_fastest
           
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
           
            struct v2f {
                float4    pos            : SV_POSITION;
                float2    uv            : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
           
            float4 _MainTex_ST;
            sampler2D _MainTex;
            float _Cutoff;
           
            v2f vert (appdata_tan v) {
                v2f o;
               
                o.pos = UnityObjectToClipPos( v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }
           
            fixed4 frag(v2f i) : COLOR {
                fixed4 tex = tex2D(_MainTex, i.uv);
               
                // If alpha is less than _Cutoff, then bail out on this pixel entirely
                // (don't even write to the depth buffer).
                clip(tex.a - _Cutoff);
 
                fixed atten = SHADOW_ATTENUATION(i); // Attenuation for shadows ONLY.
                fixed4 c = tex * atten;        // attenuate color
                c.a = tex.a;                // don't attenuate alpha
                return c;
 
            }
            ENDCG
        }
       
        // Pass to render object as a shadow caster (and receiver)
        Pass {
            Name "Caster"
            Tags { "LightMode" = "ShadowCaster" }
           
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
            #include "UnityCG.cginc"
   
            struct v2f {
                V2F_SHADOW_CASTER;
                float2  uv : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
   
            uniform float4 _MainTex_ST;
   
            v2f vert( appdata_base v )
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
   
            uniform sampler2D _MainTex;
            uniform fixed _Cutoff;
            uniform fixed4 _Color;
   
            float4 frag( v2f i ) : SV_Target
            {
                fixed4 texcol = tex2D( _MainTex, i.uv );
                clip( texcol.a*_Color.a - _Cutoff );
       
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
   
        }
 
    }
    Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}